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
    public class QuotationService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<QuotationModel> getQuotation(int id)
        {
            try
            {

                var data = (from qm in Dbcontext.QuotationMasters
                            join um in Dbcontext.UserMasters on qm.CreatedBy equals um.UID
                            join dm in Dbcontext.DataCenterMasters on qm.DataCenter equals dm.DCID
                            join cm in Dbcontext.CompanyMasters on um.CompID equals cm.CompID
                            where qm.CreatedBy == (id == 1 ? qm.CreatedBy : id)
                            //&& qm.IsActive==(id==1?qm.IsActive : true)
                            select new QuotationModel()
                            {
                                QID = qm.QID,
                                DataCenter = qm.DataCenter,
                                CableSource = qm.CableSource,
                                CableDest = qm.CableDest,
                                CableType = qm.CableType,
                                TerminationSource = qm.TerminationSource,
                                TerminationDest = qm.TerminationDest,
                                ReqDelDate = qm.ReqDelDate,
                                IsPremium = qm.IsPremium,
                                IsPatchPanel = qm.IsPatchPanel,
                                PatchPanel = qm.PatchPanel,
                                MediaConverter = qm.MediaConverter,
                                Comments = qm.Comments,
                                ServiceReq = qm.ServiceReq,
                                DateForQuote = qm.DateForQuote,
                                DateForService = qm.DateForService,
                                IsActive = qm.IsActive,
                                CreatedBy = qm.CreatedBy,
                                CreatedDate = qm.CreatedDate,
                                UpdatedBy = qm.UpdatedBy,
                                UpdatedDate = qm.UpdatedDate,
                                TicketID=qm.TicketID,
                                Prefix=qm.Prefix,
                                UserDetails = new UserModel()
                                {
                                    Title = um.Title,
                                    FirstName = um.FirstName,
                                    LastName = um.LastName
                                },
                                DCDetails = new DataCenterModel()
                                {
                                    DataCenter = dm.DataCenter
                                },
                                CompDetails = new CompanyModel()
                                {
                                    Name = cm.Name
                                }
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DataCenterModel> getDataCenter()
        {
            try
            {
                Mapper.CreateMap<DataCenterMaster, DataCenterModel>();
                List<DataCenterMaster> objCityMaster = Dbcontext.DataCenterMasters.Where(m => m.IsActive == true).ToList();
                List<DataCenterModel> objCityItem = Mapper.Map<List<DataCenterModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Insert(QuotationModel model,TicketModel model1)
        {
            try
            {                
                Mapper.CreateMap<TicketModel, TicketMaster>();
                TicketMaster objTicket = Mapper.Map<TicketMaster>(model1);
                Dbcontext.TicketMasters.Add(objTicket);
                Dbcontext.SaveChanges();

                int tid = Dbcontext.TicketMasters.Max(m => m.TicketID);
                
                Mapper.CreateMap<QuotationModel, QuotationMaster>();
                QuotationMaster objUser = Mapper.Map<QuotationMaster>(model);
                objUser.TicketID = tid;
                Dbcontext.QuotationMasters.Add(objUser);
                Dbcontext.SaveChanges();
                return tid;
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public QuotationModel getByID(int id)
        {
            try
            {
                Mapper.CreateMap<QuotationMaster, QuotationModel>();
                QuotationMaster objCityMaster = Dbcontext.QuotationMasters.Where(m => m.QID == id).SingleOrDefault();
                QuotationModel objCityItem = Mapper.Map<QuotationModel>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(QuotationModel model)
        {
            Mapper.CreateMap<QuotationModel, QuotationMaster>();
            QuotationMaster objUser = Dbcontext.QuotationMasters.SingleOrDefault(m => m.QID == model.QID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }
        public List<CompanyModel> getActiveComp()
        {
            try
            {
                Mapper.CreateMap<CompanyMaster, CompanyModel>();
                List<CompanyMaster> objCityMaster = Dbcontext.CompanyMasters.Where(m => m.Status == 1).ToList();
                List<CompanyModel> objCityItem = Mapper.Map<List<CompanyModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserModel> getUserByComp(int cid)
        {
            try
            {
                Mapper.CreateMap<UserMaster, UserModel>();
                List<UserMaster> objCityMaster = Dbcontext.UserMasters.Where(m => m.CompID == cid && m.Status == 1).ToList();
                List<UserModel> objCityItem = Mapper.Map<List<UserModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserModel> getActiveUser()
        {
            try
            {
                Mapper.CreateMap<UserMaster, UserModel>();
                List<UserMaster> objCityMaster = Dbcontext.UserMasters.Where(m => m.Role != 1 && m.Status == 1).ToList();
                List<UserModel> objCityItem = Mapper.Map<List<UserModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
