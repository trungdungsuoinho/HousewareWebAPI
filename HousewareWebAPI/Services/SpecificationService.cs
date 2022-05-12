using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HousewareWebAPI.Services
{
    public interface ISpecificationService
    {
        public Specification GetById(string id);
        public List<GetSpecByPro> GetSpecByProduct(string id);
        public bool AddValueSpecification(string productId, List<AddValueSpec> model);
        public bool DeleteValueSpecification(string productId);
        public bool UpdateValueSpecification(string productId, List<AddValueSpec> model);
        public Response GetSpecAdmin(string id);
        public Response GetAllSpecAdmin();
        public Response AddSpecAdmin(AddSpecAdminRequest model);
        public Response UpdateSpecAdmin(string id, AddSpecAdminRequest model);
        public Response DeleteSpecAdmin(string id);
        public Response ModifySort(ModifySortSpecAdminRequest model);
    }
    public class SpecificationService : ISpecificationService
    {
        private readonly HousewareContext _context;

        public SpecificationService(HousewareContext context)
        {
            _context = context;
        }

        public Specification GetById(string id)
        {
            return _context.Specifications.FirstOrDefault(s => s.SpecificationId == id.ToUpper());
        }

        public List<GetSpecByPro> GetSpecByProduct(string id)
        {
            var result = new List<GetSpecByPro>();
            var productSpecifications = _context.ProductSpecifications.Where(p => p.ProductId == id).ToList();
            if (productSpecifications != null && productSpecifications.Count > 0)
            {
                foreach (var productSpecification in productSpecifications)
                {
                    _context.Entry(productSpecification).Reference(p => p.Specification).Load();
                }
                foreach (var productSpecification in productSpecifications.OrderBy(p => p.Specification.Sort))
                {
                    result.Add(new GetSpecByPro
                    {
                        SpecificationId = productSpecification.SpecificationId,
                        Name = productSpecification.Specification.Name,
                        Description = productSpecification.Specification.Description,
                        Value = productSpecification.Value
                    });
                };
            }
            return result;
        }

        public bool AddValueSpecification(string productId, List<AddValueSpec> model)
        {
            using var transaction = _context.Database.BeginTransaction();
            foreach (var spec in model)
            {
                _context.ProductSpecifications.Add(new ProductSpecification
                {
                    ProductId = productId,
                    SpecificationId = spec.SpecificationId,
                    Value = spec.Value
                });
            }
            if (_context.SaveChanges() == model.Count)
            {
                transaction.Commit();
                return true;
            }
            else
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool DeleteValueSpecification(string productId)
        {
            using var transaction = _context.Database.BeginTransaction();
            var specifications = _context.ProductSpecifications.Where(p => p.ProductId == productId);
            foreach (var specification in specifications)
            {
                _context.ProductSpecifications.Remove(specification);
            }
            if (_context.SaveChanges() == specifications.Count())
            {
                transaction.Commit();
                return true;
            }
            else
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool UpdateValueSpecification(string productId, List<AddValueSpec> model)
        {
            if (DeleteValueSpecification(productId))
            {
                return AddValueSpecification(productId, model);
            }
            return false;
        }

        public Response GetSpecAdmin(string id)
        {
            var response = new Response();
            try
            {
                var specification = _context.Specifications.FirstOrDefault(s => s.SpecificationId == id.ToUpper());
                if (specification == null)
                {
                    response.SetCode(CodeTypes.Err_NotFound);
                    response.SetResult("No Product was found for this ProductId");
                }
                else
                {
                    var result = new GetSpecAdminResponse()
                    {
                        SpecificationId = specification.SpecificationId,
                        Name = specification.Name,
                        Description = specification.Description
                    };
                    response.SetCode(CodeTypes.Success);
                    response.SetResult(result);
                }
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response GetAllSpecAdmin()
        {
            var response = new Response();
            try
            {
                var specifications = _context.Specifications.OrderBy(c => c.Sort).ToList();
                var result = new List<GetSpecAdminResponse>();
                foreach (var specification in specifications)
                {
                    result.Add(new GetSpecAdminResponse()
                    {
                        SpecificationId = specification.SpecificationId,
                        Name = specification.Name,
                        Description = specification.Description
                    });
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(result);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response AddSpecAdmin(AddSpecAdminRequest model)
        {
            var response = new Response();
            try
            {
                if (GetById(model.SpecificationId) == null)
                {
                    var specification = new Specification()
                    {
                        SpecificationId = model.SpecificationId,
                        Name = model.Name,
                        Description = model.Description
                    };
                    _context.Specifications.Add(specification);
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                }
                else
                {
                    response.SetCode(CodeTypes.Err_Exist);
                    response.SetResult("There already exists a Specification with such SpecificationId");
                }
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response UpdateSpecAdmin(string id, AddSpecAdminRequest model)
        {
            var response = new Response();
            try
            {
                if (string.Compare(id, model.SpecificationId, true) != 0)
                {
                    response.SetCode(CodeTypes.Err_IdNotMatch);
                    response.SetResult("SpecificationId in URL doesn't match SpecificationId in model");
                    return response;
                }

                var specification = GetById(model.SpecificationId);
                if (specification != null)
                {
                    specification.Name = model.Name;
                    specification.Description = model.Description;
                    _context.Entry(specification).State = EntityState.Modified;
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                }
                else
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Specification with such SpecificationId");
                }
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response DeleteSpecAdmin(string id)
        {
            var response = new Response();
            try
            {
                var specification = GetById(id);
                if (specification != null)
                {
                    _context.Entry(specification).State = EntityState.Deleted;
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                    return response;
                }
                response.SetCode(CodeTypes.Err_NotExist);
                response.SetResult("There not exists a Specification with such SpecificationId");
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response ModifySort(ModifySortSpecAdminRequest model)
        {
            var response = new Response();
            var specifications = _context.Specifications.ToList();
            foreach (var specification in specifications)
            {
                var id = model.SpecificationIds.Where(i => i.ToUpper() == specification.SpecificationId).FirstOrDefault();
                specification.Sort = id != null ? model.SpecificationIds.IndexOf(id) : int.MaxValue;
                _context.Entry(specification).State = EntityState.Modified;
            }
            _context.SaveChanges();
            response.SetCode(CodeTypes.Success);
            return response;
        }
    }
}
