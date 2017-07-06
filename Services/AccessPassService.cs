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
    public class AccessPassService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<AccessPassModel> getAPData(int cid)
        {
            try
            {
                //int cy = System.DateTime.Now.Year;
                //int curm = System.DateTime.Now.Month;
                //int lm = System.DateTime.Now.AddMonths(-1).Month;
                //int ly = System.DateTime.Now.AddYears(-1).Year;
                var maxRaw = Dbcontext.AccessPassMasters.OrderByDescending(m => m.SwipeDate).FirstOrDefault();
                int mm = 0;
                int yy = 0;
                if (maxRaw != null)
                {
                    mm = maxRaw.SwipeDate.Value.Month;
                    yy = maxRaw.SwipeDate.Value.Year;
                }
                //Mapper.CreateMap<PowerUsageMaster, PowerUsageModel>();
                //List<PowerUsageMaster> objCityMaster = Dbcontext.PowerUsageMasters.Where(m => m.Status == 1).ToList();
                //List<PowerUsageModel> objCityItem = Mapper.Map<List<PowerUsageModel>>(objCityMaster);
                //return objCityItem;
                var data = (from ap in Dbcontext.AccessPassMasters
                            join cm in Dbcontext.CompanyMasters on ap.CompID equals cm.CompID into comp
                            from c in comp.DefaultIfEmpty()
                            where ap.CompID == (cid == 0 ? ap.CompID : cid) && (ap.SwipeDate.Value.Month) == mm && (ap.SwipeDate.Value.Year) == yy
                            select new AccessPassModel()
                            {
                                APID=ap.APID,
                                CompID=ap.CompID,
                                FirstName=ap.FirstName,
                                LastName=ap.LastName,
                                AccessPassID=ap.AccessPassID,
                                SwipeDate=ap.SwipeDate,
                                SwipeTime=ap.SwipeTime,
                                DoorID=ap.DoorID,
                                CompDetails = new CompanyModel()
                                {
                                    Name = (c == null ? string.Empty : c.Name)
                                }
                            }).ToList();
                return data;              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AccessPassModel> getAPDataByMMYY(int cid,int mm,int yy)
        {
            try
            {
                //int cy = System.DateTime.Now.Year;
                //int curm = System.DateTime.Now.Month;
                //int lm = System.DateTime.Now.AddMonths(-1).Month;
                //int ly = System.DateTime.Now.AddYears(-1).Year;
                //var maxRaw = Dbcontext.AccessPassMasters.OrderByDescending(m => m.SwipeDate).FirstOrDefault();
                //int mm = maxRaw.SwipeDate.Value.Month;
                //int yy = maxRaw.SwipeDate.Value.Year;
                //Mapper.CreateMap<PowerUsageMaster, PowerUsageModel>();
                //List<PowerUsageMaster> objCityMaster = Dbcontext.PowerUsageMasters.Where(m => m.Status == 1).ToList();
                //List<PowerUsageModel> objCityItem = Mapper.Map<List<PowerUsageModel>>(objCityMaster);
                //return objCityItem;
                var data = (from ap in Dbcontext.AccessPassMasters
                            join cm in Dbcontext.CompanyMasters on ap.CompID equals cm.CompID into comp
                            from c in comp.DefaultIfEmpty()
                            where ap.CompID == (cid == 0 ? ap.CompID : cid) && (ap.SwipeDate.Value.Month) == mm && (ap.SwipeDate.Value.Year) == yy
                            select new AccessPassModel()
                            {
                                APID = ap.APID,
                                CompID = ap.CompID,
                                FirstName = ap.FirstName,
                                LastName = ap.LastName,
                                AccessPassID = ap.AccessPassID,
                                SwipeDate = ap.SwipeDate,
                                SwipeTime = ap.SwipeTime,
                                DoorID = ap.DoorID,
                                CompDetails = new CompanyModel()
                                {
                                    Name = (c == null ? string.Empty : c.Name)
                                }
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Insert(PowerUsageModel model)
        {
            try
            {
                Mapper.CreateMap<PowerUsageModel, PowerUsageMaster>();
                PowerUsageMaster objUser = Mapper.Map<PowerUsageMaster>(model);
                Dbcontext.PowerUsageMasters.Add(objUser);
                return Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AccessPassModel getByID(int id)
        {
            try
            {
                Mapper.CreateMap<AccessPassMaster, AccessPassModel>();
                AccessPassMaster objCityMaster = Dbcontext.AccessPassMasters.Where(m => m.APID == id).SingleOrDefault();
                AccessPassModel objCityItem = Mapper.Map<AccessPassModel>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(AccessPassModel model)
        {
            Mapper.CreateMap<AccessPassModel, AccessPassMaster>();
            AccessPassMaster objUser = Dbcontext.AccessPassMasters.SingleOrDefault(m => m.APID == model.APID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }
    }
}
