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
    public class DashboardService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<TicketModel> getAllTicket()
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
                            where tm.TicketStatusID!=2
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
                                Prefix=tm.Prefix,
                                UserDetails = new UserModel()
                                {
                                    Title = um.Title,
                                    FirstName = um.FirstName,
                                    LastName = um.LastName
                                },
                                CompDetails = new CompanyModel()
                                {
                                    Name = (c==null?string.Empty:c.Name)
                                },
                                TStatusDetails = new TicketStatusModel()
                                {
                                    TicketStatus = ts.TicketStatus
                                },
                                TTypeDetails = new TicketTypeModel()
                                {
                                    TicketType=tt.TicketType
                                }
                            }).OrderByDescending(m => m.CreatedDate).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
