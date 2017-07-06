using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.ViewModel;

namespace Services
{
    public class ReportService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<TicketModel> getTicketData(DateTime todt, DateTime frmdt, int uid,int cid)
        {
            try
            {
                //Mapper.CreateMap<TicketMaster, TicketModel>();
                //List<TicketMaster> objCityMaster = Dbcontext.TicketMasters.Where(m=>m.CreatedBy==(uid==1?m.CreatedBy:uid)).ToList();
                //List<TicketModel> objCityItem = Mapper.Map<List<TicketModel>>(objCityMaster);
                //return objCityItem;

                var data = (from tm in Dbcontext.TicketMasters
                            join tsm in Dbcontext.TicketStatusMasters on tm.TicketStatusID equals tsm.TicketStatusID
                            join um in Dbcontext.UserMasters on tm.CreatedBy equals um.UID
                            join cm in Dbcontext.CompanyMasters on um.CompID equals cm.CompID
                            where tm.CreatedDate >= frmdt && tm.CreatedDate <= todt && tm.CreatedBy == (uid==0?tm.CreatedBy:uid) && um.CompID==(cid==0?um.CompID:cid)
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
                                },
                                CompDetails = new CompanyModel()
                                {
                                    Name = cm.Name
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
