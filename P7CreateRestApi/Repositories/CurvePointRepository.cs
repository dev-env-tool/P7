
//using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using System.Collections;

namespace P7CreateRestApi.Repositories
{
    public class CurvePointRepository : ICurvePointRepository
    {

        private static P7Referential? _context;

        public CurvePointRepository(P7Referential context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CurvePoint>> GetAllCurvePoints()
        {
            return await _context!.CurvePoints.ToListAsync();
        }

        public async Task<IEnumerable<CurvePoint>> GetCurvePointById(int id)
        {
            return await _context!.CurvePoints.Where(c => c.Id == id)
                                  .ToListAsync();
        }

        public void CreateCurvePoint(CurvePoint curvePoint)
        {
            if (curvePoint != null)
            {
                _context!.CurvePoints.Add(curvePoint);
                _context.SaveChanges();
            }
        }
        public async Task UpdateCurvePoint(CurvePoint curvePoint)
        {
            int maxCurvePointId = await GetMaxCurvePointId();
            if (curvePoint != null)
            {
                if ((curvePoint.Id > 0) && (curvePoint.Id <= maxCurvePointId))
                { 
                    _context!.Entry(curvePoint).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }

        public void DeleteCurvePointById(int id)
        {
            CurvePoint CurvePoint = _context!.CurvePoints.First(c => c.Id == id);

            if (CurvePoint != null)
            {
                _context!.CurvePoints.Remove(CurvePoint);
                _context.SaveChanges();
            }
        }

        private static async Task<int> GetMaxCurvePointId()
        {
            int maxCurvePointId = _context!.CurvePoints.Select(c => c.Id).Max();
            return await Task.FromResult(maxCurvePointId);
        }
    }
}