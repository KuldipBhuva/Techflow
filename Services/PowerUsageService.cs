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
    public class PowerUsageService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<PowerUsageModel> getPUData(int cid)
        {
            try
            {

                int mm = 0;
                int yy = 0;
                var maxRaw = Dbcontext.PowerUsageMasters.OrderByDescending(m => m.LastReading).Where(m => m.CompID == (cid == 0 ? m.CompID : cid)).FirstOrDefault();
                if (maxRaw != null)
                {
                    mm = maxRaw.LastReading.Value.Month;
                    yy = maxRaw.LastReading.Value.Year;
                }
                var data = (from pu in Dbcontext.PowerUsageMasters
                            join cm in Dbcontext.CompanyMasters on pu.CompID equals cm.CompID into comp
                            from c in comp.DefaultIfEmpty()
                            where pu.CompID == (cid == 0 ? pu.CompID : cid) && (pu.LastReading.Value.Month) == mm && (pu.LastReading.Value.Year) == yy
                            select new PowerUsageModel()
                            {
                                PUID = pu.PUID,
                                CompID = pu.CompID,
                                DataSuite = pu.DataSuite,
                                RackID = pu.RackID,
                                PowerFeeds = pu.PowerFeeds,
                                AmpereRating = pu.AmpereRating,
                                PowerContracted = pu.PowerContracted,
                                PowerUsed = pu.PowerUsed,
                                OverUnder = pu.OverUnder,
                                LastReading = pu.LastReading,

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
        public List<PowerUsageModel> getPUDataByMMYY(int mon, int year, int cid)
        {
            try
            {
                //Mapper.CreateMap<PowerUsageMaster, PowerUsageModel>();
                //List<PowerUsageMaster> objCityMaster = Dbcontext.PowerUsageMasters.Where(m => m.Status == 1).ToList();
                //List<PowerUsageModel> objCityItem = Mapper.Map<List<PowerUsageModel>>(objCityMaster);
                //return objCityItem;
                //var maxRaw = Dbcontext.PowerUsageMasters.OrderByDescending(m => m.LastReading).FirstOrDefault();
                //int mm = maxRaw.LastReading.Value.Month;
                //int yy = maxRaw.LastReading.Value.Year;
                var data = (from pu in Dbcontext.PowerUsageMasters
                            join cm in Dbcontext.CompanyMasters on pu.CompID equals cm.CompID into comp
                            from c in comp.DefaultIfEmpty()
                            //where pu.CompID == (cid == 0 ? pu.CompID : cid) && (pu.LastReading.Value.Month) == mon && (pu.LastReading.Value.Year) == year
                            select new PowerUsageModel()
                            {
                                PUID = pu.PUID,
                                CompID = pu.CompID,
                                DataSuite = pu.DataSuite,
                                RackID = pu.RackID,
                                PowerFeeds = pu.PowerFeeds,
                                AmpereRating = pu.AmpereRating,
                                PowerContracted = pu.PowerContracted,
                                PowerUsed = pu.PowerUsed,
                                OverUnder = pu.OverUnder,
                                LastReading = pu.LastReading,

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
        public PowerUsageModel getByID(int id)
        {
            try
            {
                Mapper.CreateMap<PowerUsageMaster, PowerUsageModel>();
                PowerUsageMaster objCityMaster = Dbcontext.PowerUsageMasters.Where(m => m.PUID == id).SingleOrDefault();
                PowerUsageModel objCityItem = Mapper.Map<PowerUsageModel>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(PowerUsageModel model)
        {
            Mapper.CreateMap<PowerUsageModel, PowerUsageMaster>();
            PowerUsageMaster objUser = Dbcontext.PowerUsageMasters.SingleOrDefault(m => m.PUID == model.PUID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }
        public List<PowerUsageModel> getTotalPU(string year,int cid)
        {
            //List<PDeclareModel> duePayment = Dbcontext.Database.SqlQuery<PDeclareModel>("exec SP_Payment").ToList<PDeclareModel>();
            //return duePayment;            
            var yr = new SqlParameter("@yr", year);
            var comp = new SqlParameter("@cid", cid);
            List<PowerUsageModel> duePayment = Dbcontext.Database.SqlQuery<PowerUsageModel>("SP_TotalPU @yr,@cid", yr,comp).ToList<PowerUsageModel>();
            return duePayment;
        }
    }
}
