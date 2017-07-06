using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Models;
using Models.ViewModel;

namespace Services
{
    public class TicketService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<TicketModel> getTicket(int uid, int rid)
        {
            try
            {
                //Mapper.CreateMap<TicketMaster, TicketModel>();
                //List<TicketMaster> objCityMaster = Dbcontext.TicketMasters.Where(m=>m.CreatedBy==(uid==1?m.CreatedBy:uid)).ToList();
                //List<TicketModel> objCityItem = Mapper.Map<List<TicketModel>>(objCityMaster);
                //return objCityItem;

                var data = (from tm in Dbcontext.TicketMasters
                            join tsm in Dbcontext.TicketStatusMasters on tm.TicketStatusID equals tsm.TicketStatusID
                            where tm.CreatedBy == (rid == 1 ? tm.CreatedBy : uid)
                            select new TicketModel()
                            {
                                TicketID = tm.TicketID,
                                Subject = tm.Subject,
                                Description = tm.Description,
                                AssignTo = tm.AssignTo,
                                CreatedBy = tm.CreatedBy,
                                CreatedDate = tm.CreatedDate,
                                UpdatedBy = tm.UpdatedBy,
                                UpdatedDate = tm.UpdatedDate,
                                IsActive = tm.IsActive,
                                SenderEmail = tm.SenderEmail,
                                TicketStatusID = tm.TicketStatusID,
                                TicketTypeID = tm.TicketTypeID,
                                Priority = tm.Priority,
                                Prefix = tm.Prefix,
                                TStatusDetails = new TicketStatusModel()
                                {
                                    TicketStatus = tsm.TicketStatus
                                }
                            }).OrderByDescending(m => m.CreatedDate).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertTicket(TicketModel model)
        {
            try
            {
                Mapper.CreateMap<TicketModel, TicketMaster>();
                TicketMaster objUser = Mapper.Map<TicketMaster>(model);
                Dbcontext.TicketMasters.Add(objUser);
                Dbcontext.SaveChanges();
                int tid = Dbcontext.TicketMasters.Max(m => m.TicketID);
                return tid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TicketModel GetLastID()
        {
            Mapper.CreateMap<TicketMaster, TicketModel>();
            TicketMaster objCityMaster = Dbcontext.TicketMasters.OrderByDescending(p => p.TicketID).FirstOrDefault();
            TicketModel objCityItem = Mapper.Map<TicketModel>(objCityMaster);
            return objCityItem;
         
        }
        public int InsertTicketTran(TicketTranModel model)
        {
            try
            {
                Mapper.CreateMap<TicketTranModel, TicketTran>();
                TicketTran objUser = Mapper.Map<TicketTran>(model);
                Dbcontext.TicketTrans.Add(objUser);
                Dbcontext.SaveChanges();
                int ttid = Dbcontext.TicketTrans.Max(m => m.TTranID);
                return ttid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public int InsertTicketAttachment(TicketAttachmentModel model)
        //{
        //    try
        //    {
        //        Mapper.CreateMap<TicketAttachmentModel, TicketAttachment>();
        //        TicketAttachment objUser = Mapper.Map<TicketAttachment>(model);
        //        Dbcontext.TicketAttachments.Add(objUser);
        //        return Dbcontext.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<TicketTypeModel> getTicketType()
        {
            try
            {
                Mapper.CreateMap<TicketTypeMaster, TicketTypeModel>();
                List<TicketTypeMaster> objCityMaster = Dbcontext.TicketTypeMasters.Where(m => m.IsActive == true).ToList();
                List<TicketTypeModel> objCityItem = Mapper.Map<List<TicketTypeModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TicketStatusModel> getTicketStatus()
        {
            try
            {
                Mapper.CreateMap<TicketStatusMaster, TicketStatusModel>();
                List<TicketStatusMaster> objCityMaster = Dbcontext.TicketStatusMasters.Where(m => m.IsActive == true).ToList();
                List<TicketStatusModel> objCityItem = Mapper.Map<List<TicketStatusModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public QuotationModel getQuoteByTIcketID(int id)
        {
            try
            {
                Mapper.CreateMap<QuotationMaster, QuotationModel>();
                QuotationMaster objCityMaster = Dbcontext.QuotationMasters.Where(m => m.TicketID == id).SingleOrDefault();
                QuotationModel objCityItem = Mapper.Map<QuotationModel>(objCityMaster);
                return objCityItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DeliveryModel getDelByTIcketID(int id)
        {
            try
            {
                Mapper.CreateMap<DeliveryMaster, DeliveryModel>();
                DeliveryMaster objCityMaster = Dbcontext.DeliveryMasters.Where(m => m.TicketID == id).SingleOrDefault();
                DeliveryModel objCityItem = Mapper.Map<DeliveryModel>(objCityMaster);
                return objCityItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AccessModel getAccessByTIcketID(int id)
        {
            try
            {
                Mapper.CreateMap<AccessMaster, AccessModel>();
                AccessMaster objCityMaster = Dbcontext.AccessMasters.Where(m => m.TicketID == id).SingleOrDefault();
                AccessModel objCityItem = Mapper.Map<AccessModel>(objCityMaster);
                return objCityItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TicketModel getTicketByID(int id)
        {
            try
            {
                //Mapper.CreateMap<TicketMaster, TicketModel>();
                //List<TicketMaster> objCityMaster = Dbcontext.TicketMasters.ToList();
                //List<TicketModel> objCityItem = Mapper.Map<List<TicketModel>>(objCityMaster);
                //return objCityItem;
                var data = (from tm in Dbcontext.TicketMasters
                            join um in Dbcontext.UserMasters on tm.CreatedBy equals um.UID
                            join cm in Dbcontext.CompanyMasters on um.CompID equals cm.CompID into cmp
                            from c in cmp.DefaultIfEmpty()
                            join tt in Dbcontext.TicketTypeMasters on tm.TicketTypeID equals tt.TicketTypeID
                            join ts in Dbcontext.TicketStatusMasters on tm.TicketStatusID equals ts.TicketStatusID
                            where tm.TicketID == id
                            select new TicketModel()
                            {
                                TicketID = tm.TicketID,
                                Subject = tm.Subject,
                                Description = tm.Description,
                                AssignTo = tm.AssignTo,
                                CreatedBy = tm.CreatedBy,
                                CreatedDate = tm.CreatedDate,
                                UpdatedBy = tm.UpdatedBy,
                                UpdatedDate = tm.UpdatedDate,
                                IsActive = tm.IsActive,
                                SenderEmail = tm.SenderEmail,
                                TicketStatusID = tm.TicketStatusID,
                                TicketTypeID = tm.TicketTypeID,
                                Priority = tm.Priority,
                                Prefix = tm.Prefix,
                                UserDetails = new UserModel()
                                {
                                    Title = um.Title,
                                    FirstName = um.FirstName,
                                    LastName = um.LastName
                                },
                                CompDetails = new CompanyModel()
                                {
                                    Name = (c == null ? string.Empty : c.Name)
                                },
                                TStatusDetails = new TicketStatusModel()
                                {
                                    TicketStatus = ts.TicketStatus
                                },
                                TTypeDetails = new TicketTypeModel()
                                {
                                    TicketType = tt.TicketType
                                }
                            }).SingleOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<TicketTranModel> getTicketTran(int uid, int tid, int rid)
        //{
        //    //List<PDeclareModel> duePayment = Dbcontext.Database.SqlQuery<PDeclareModel>("exec SP_Payment").ToList<PDeclareModel>();
        //    //return duePayment;            
        //    var user = new SqlParameter("@uid", uid);
        //    var tik = new SqlParameter("@tid", tid);
        //    var role = new SqlParameter("@rid", rid);
        //    List<TicketTranModel> duePayment = Dbcontext.Database.SqlQuery<TicketTranModel>("SP_getTicketTran @uid,@tid,@tik", user, tik, role).ToList<TicketTranModel>();
        //    return duePayment;
        //}
        //public List<TicketTranModel> getTicketTran(int uid, int tid, int rid)
        //{
        //    object[] params1 = {
        //        new SqlParameter("@uid", uid),
        //        new SqlParameter("@tid", tid),
        //                       new SqlParameter("@rid",rid)};
        //    //Get student name of string type
        //    //var Repayment = Dbcontext.Database.SqlQuery<TicketTranModel>
        //    //    ("EXEC SP_getTicketTran @uid,@tid,@rid", new SqlParameter("uid", uid), new SqlParameter("tid", tid), new SqlParameter("rid", rid)).ToList<TicketTranModel>();
        //    var Repayment = Dbcontext.Database.SqlQuery<TicketTranModel>("exec SP_getTicketTran", params1).ToList<TicketTranModel>();
        //    return Repayment;
        //}
        public List<TicketTranModel> getTicketTran(int uid, int tid, int rid)
        {
            try
            {
                //Mapper.CreateMap<TicketTran, TicketTranModel>();
                //List<TicketTran> objCityMaster = Dbcontext.TicketTrans.Where(m => m.CreatedBy == (rid == 1 ? m.CreatedBy : uid) && m.TicketID == tid).ToList();
                //List<TicketTranModel> objCityItem = Mapper.Map<List<TicketTranModel>>(objCityMaster);
                //return objCityItem;

                var data = (from tt in Dbcontext.TicketTrans
                            join um in Dbcontext.UserMasters on tt.ReplayBy equals um.UID into usr
                            from u in usr.DefaultIfEmpty()
                            where tt.CreatedBy == (rid == 1 ? tt.CreatedBy : uid) && tt.TicketID == tid
                            select new TicketTranModel()
                            {
                                TTranID = tt.TTranID,
                                TicketID = tt.TicketID,
                                AssignTo = tt.AssignTo,
                                Comment = tt.Comment,
                                FileName = tt.FileName,
                                FileURL = tt.FileURL,
                                ReplayBy = tt.ReplayBy,
                                ReplayDate = tt.ReplayDate,
                                CreatedBy = tt.CreatedBy,
                                CreatedDate = tt.CreatedDate,
                                Prefix = tt.Prefix,
                                UserDetails = new UserModel()
                                {
                                    Role = (u == null ? int.MinValue : u.Role)

                                }
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<TicketAttachmentModel> getTicketAttachments(int uid)
        //{
        //    try
        //    {
        //        //Mapper.CreateMap<TicketAttachment, TicketAttachmentModel>();
        //        //List<TicketAttachment> objCityMaster = Dbcontext.TicketAttachments.Where(m => m.ReplayBy == true).ToList();
        //        //List<TicketAttachmentModel> objCityItem = Mapper.Map<List<TicketAttachmentModel>>(objCityMaster);
        //        //return objCityItem;

        //        var data = (from ta in Dbcontext.TicketAttachments
        //                    join tm in Dbcontext.TicketMasters on ta.TID equals tm.TicketID
        //                    where tm.CreatedBy == (uid == 1 ? tm.CreatedBy : uid)
        //                    select new TicketAttachmentModel()
        //                    {
        //                        TAID=ta.TAID,
        //                        TID=ta.TID,
        //                        FileName=ta.FileName,
        //                        FileURL=ta.FileURL,
        //                        ReplayBy=ta.ReplayBy,
        //                        ReplayDate=ta.ReplayDate
        //                    }).ToList();
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
