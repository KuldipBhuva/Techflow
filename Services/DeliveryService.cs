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
    public class DeliveryService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<DeliveryModel> getDeliveryReq(int id)
        {
            try
            {
                //Mapper.CreateMap<AccessMaster, AccessModel>();
                //List<AccessMaster> objCityMaster = Dbcontext.AccessMasters.ToList();
                //List<AccessModel> objCityItem = Mapper.Map<List<AccessModel>>(objCityMaster);
                //return objCityItem;
                var data = (from dm in Dbcontext.DeliveryMasters
                            join um in Dbcontext.UserMasters on dm.CreatedBy equals um.UID
                            join dcm in Dbcontext.DataCenterMasters on dm.DataCenter equals dcm.DCID
                            where dm.CreatedBy == (id == 1 ? dm.CreatedBy : id)
                            select new DeliveryModel()
                            {
                                BDID=dm.BDID,
                                DataCenter=dm.DataCenter,
                                FirstName=dm.FirstName,
                                LastName=dm.LastName,
                                Company=dm.Company,
                                StDateTime=dm.StDateTime,
                                EndDateTime=dm.EndDateTime,                               
                                Boxes=dm.Boxes,
                                TrackingNo=dm.TrackingNo,
                                ProjectRef=dm.ProjectRef,
                                Storage=dm.Storage,
                                RemoteHands=dm.RemoteHands,
                                IsActive=dm.IsActive,
                                CreatedBy=dm.CreatedBy,
                                CreatedDate=dm.CreatedDate,
                                TicketID=dm.TicketID,
                                Prefix=dm.Prefix,
                                UserDetails = new UserModel()
                                {
                                    Title = um.Title,
                                    FirstName = um.FirstName,
                                    LastName = um.LastName
                                },
                                DCDetails = new DataCenterModel()
                                {
                                    DataCenter = dcm.DataCenter
                                }
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public int Insert(DeliveryModel model,TicketModel model1)
        {
            try
            {
                Mapper.CreateMap<TicketModel, TicketMaster>();
                TicketMaster objUser1 = Mapper.Map<TicketMaster>(model1);
                Dbcontext.TicketMasters.Add(objUser1);
                Dbcontext.SaveChanges();

                int tid = Dbcontext.TicketMasters.Max(m => m.TicketID);

                Mapper.CreateMap<DeliveryModel, DeliveryMaster>();
                DeliveryMaster objUser = Mapper.Map<DeliveryMaster>(model);
                objUser.TicketID = tid;
                Dbcontext.DeliveryMasters.Add(objUser);
                Dbcontext.SaveChanges();
                return tid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DeliveryModel getByID(int id)
        {
            try
            {
                Mapper.CreateMap<DeliveryMaster, DeliveryModel>();
                DeliveryMaster objCityMaster = Dbcontext.DeliveryMasters.Where(m => m.BDID == id).SingleOrDefault();
                DeliveryModel objCityItem = Mapper.Map<DeliveryModel>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(DeliveryModel model)
        {
            Mapper.CreateMap<DeliveryModel, DeliveryMaster>();
            DeliveryMaster objUser = Dbcontext.DeliveryMasters.SingleOrDefault(m => m.BDID == model.BDID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }
    }
}
