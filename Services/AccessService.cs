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
    public class AccessService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<AccessModel> getAccessReq(int id)
        {
            try
            {
                //Mapper.CreateMap<AccessMaster, AccessModel>();
                //List<AccessMaster> objCityMaster = Dbcontext.AccessMasters.ToList();
                //List<AccessModel> objCityItem = Mapper.Map<List<AccessModel>>(objCityMaster);
                //return objCityItem;
                var data = (from am in Dbcontext.AccessMasters
                            join um in Dbcontext.UserMasters on am.CreatedBy equals um.UID
                            join dm in Dbcontext.DataCenterMasters on am.DataCenter equals dm.DCID
                            join cm in Dbcontext.CompanyMasters on um.CompID equals cm.CompID into cc
                            from c in cc.DefaultIfEmpty()
                            where c.CompID ==(id == 0 ? c.CompID : id)
                            select new AccessModel()
                            {
                                BAID = am.BAID,
                                DataCenter = am.DataCenter,
                                FirstName = am.FirstName,
                                LastName = am.LastName,
                                Company = am.Company,
                                StDateTime = am.StDateTime,
                                EndDateTime = am.EndDateTime,
                                Reason = am.Reason,
                                IsActive = am.IsActive,
                                CreatedBy = am.CreatedBy,
                                CreatedDate = am.CreatedDate,
                                UpdatedBy = am.UpdatedBy,
                                UpdatedDate = am.UpdatedDate,
                                TicketID=am.TicketID,
                                Prefix=am.Prefix,
                                UserDetails = new UserModel()
                                {
                                    Title = um.Title,
                                    FirstName = um.FirstName,
                                    LastName = um.LastName
                                },
                                DCDetails = new DataCenterModel()
                                {
                                    DataCenter = dm.DataCenter
                                }
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public int Insert(AccessModel model,TicketModel model1)
        {
            try
            {                
                Mapper.CreateMap<TicketModel, TicketMaster>();
                TicketMaster objUser1 = Mapper.Map<TicketMaster>(model1);                
                Dbcontext.TicketMasters.Add(objUser1);
                Dbcontext.SaveChanges();

                int tid = Dbcontext.TicketMasters.Max(m => m.TicketID);

                Mapper.CreateMap<AccessModel, AccessMaster>();
                AccessMaster objUser = Mapper.Map<AccessMaster>(model);
                objUser.TicketID = tid;
                Dbcontext.AccessMasters.Add(objUser);
                Dbcontext.SaveChanges();
                return tid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public AccessModel getByID(int id)
        {
            try
            {
                Mapper.CreateMap<AccessMaster, AccessModel>();
                AccessMaster objCityMaster = Dbcontext.AccessMasters.Where(m => m.BAID == id).SingleOrDefault();
                AccessModel objCityItem = Mapper.Map<AccessModel>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(AccessModel model)
        {
            Mapper.CreateMap<AccessModel, AccessMaster>();
            AccessMaster objUser = Dbcontext.AccessMasters.SingleOrDefault(m => m.BAID == model.BAID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }
    }
}
