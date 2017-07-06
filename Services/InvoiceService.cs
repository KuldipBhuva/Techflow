using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Models;
using Models.ViewModel;

namespace Services
{
    public class InvoiceService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<InvoiceModel> getInvoice(int cid,int rid)
        {
            try
            {
                //Mapper.CreateMap<InvoiceMaster, InvoiceModel>();
                //List<InvoiceMaster> objCityMaster = Dbcontext.InvoiceMasters.ToList();
                //List<InvoiceModel> objCityItem = Mapper.Map<List<InvoiceModel>>(objCityMaster);
                //return objCityItem;

                var data = (from im in Dbcontext.InvoiceMasters
                            join cm in Dbcontext.CompanyMasters on im.CompID equals cm.CompID
                            where im.CompID == (rid == 1 ? im.CompID : cid)
                            select new InvoiceModel()
                            {
                                IID = im.IID,
                                CompID = im.CompID,
                                Amount = im.Amount,
                                DueDate = im.DueDate,
                                Status = im.Status,
                                FileName = im.FileName,
                                FileURL = im.FileURL,
                                CompDetails = new CompanyModel()
                                {
                                    Name=cm.Name
                                }
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Insert(InvoiceModel model)
        {
            try
            {
                Mapper.CreateMap<InvoiceModel, InvoiceMaster>();
                InvoiceMaster objUser = Mapper.Map<InvoiceMaster>(model);
                Dbcontext.InvoiceMasters.Add(objUser);
                return Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public InvoiceModel getByID(int id)
        {
            try
            {
                Mapper.CreateMap<InvoiceMaster, InvoiceModel>();
                InvoiceMaster objCityMaster = Dbcontext.InvoiceMasters.Where(m => m.IID == id).SingleOrDefault();
                InvoiceModel objCityItem = Mapper.Map<InvoiceModel>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(InvoiceModel model)
        {
            Mapper.CreateMap<InvoiceModel, InvoiceMaster>();
            InvoiceMaster objUser = Dbcontext.InvoiceMasters.SingleOrDefault(m => m.IID == model.IID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }

        public bool DeleteByID(int id)
        {
            try
            {
                Mapper.CreateMap<InvoiceMaster, InvoiceModel>();
                InvoiceMaster objCityMaster = Dbcontext.InvoiceMasters.Where(m => m.IID == id).SingleOrDefault();
                Dbcontext.InvoiceMasters.Remove(objCityMaster);
                Dbcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
