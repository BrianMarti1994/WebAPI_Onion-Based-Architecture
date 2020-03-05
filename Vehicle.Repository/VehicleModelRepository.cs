﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.DAL;
using Vehicle.Common.Common;

using Vehicle.Model.Common;
using Vehicle.Repository.Common;
using AutoMapper;
using Vehicle.Common;

namespace Vehicle.Repository
{
   public class VehicleModelRepository : Repository<Model.VehicleModel>, IVehicleModelRepository
    {
        Generic objGen = new Generic();

        public VehicleModelRepository(VehicleDbEntities context) : base(context)
        {
        }


        public async Task<List<IVehicleModel>> GetAllVehiclesModel(PaginatedInputModel pagingParams)
        {
            List<IVehicleModel> list = new List<IVehicleModel>();

            try
            {

                using (var unitOfWork = new UnitOfWork(new VehicleDbEntities()))
                {
                    string FilterValue = string.Empty, StortingCol = string.Empty;
                    IEnumerable<string> disFilterValue = pagingParams.FilterParam.Where(x => !String.IsNullOrEmpty(x.FilterValue)).Select(x => x.FilterValue).Distinct();
                    foreach (string FilterVal in disFilterValue)
                    {
                        FilterValue = FilterVal;
                    }
                    if (!string.IsNullOrEmpty(FilterValue))
                    {

                        var objd = unitOfWork.VehicleModels.GetAll().Where(s => s.Name.Contains(FilterValue) || s.Abrv.Contains(FilterValue))
                            .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                             .Take(pagingParams.PageSize)
                             .ToList();
                        list = Mapper.Map<List<IVehicleModel>>(objd);

                        return await Task.FromResult(list);
                    }
                    foreach (var sortingParam in pagingParams.SortingParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)))
                    {
                        StortingCol = sortingParam.ColumnName;
                    }


                    switch (StortingCol)
                    {

                        case "Id":
                            var obId = unitOfWork.VehicleModels.GetAll().OrderByDescending(x => x.Id).Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                             .Take(pagingParams.PageSize)
                             .ToList();
                            list = Mapper.Map<List<IVehicleModel>>(obId);
                            break;
                        case "MakeId":
                            var obMakeId = unitOfWork.VehicleModels.GetAll()
                                .OrderByDescending(x => x.MakeId).Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                             .Take(pagingParams.PageSize)
                             .ToList();
                            list = Mapper.Map<List<IVehicleModel>>(obMakeId);
                            break;
                        case "Name":
                            var obNam = unitOfWork.VehicleModels.GetAll().OrderByDescending(x => x.Name)
                                .OrderByDescending(x => x.Name)
                              .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                             .Take(pagingParams.PageSize)
                             .ToList();
                            list = Mapper.Map<List<IVehicleModel>>(obNam);
                            break;

                        case "Abrv":
                            var obAb = unitOfWork.VehicleModels.GetAll().OrderByDescending(x => x.Abrv)
                                .OrderByDescending(x => x.Abrv)
                               .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                             .Take(pagingParams.PageSize)
                             .ToList();
                            list = Mapper.Map<List<IVehicleModel>>(obAb);
                            break;

                        default:
                            var objd = unitOfWork.VehicleModels.GetAll().OrderBy(c => c.Name)
                                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                             .Take(pagingParams.PageSize)
                             .ToList();
                            list = Mapper.Map<List<IVehicleModel>>(objd);
                            break;

                    }
                    return await Task.FromResult(list);

                }
            }
            catch (Exception ex)
            {
                //Logs can be stored into Database or can be Email to developer.

                objGen.ErrorLogging(ex, "GetAllVehiclesModel");

                return list;
            }
        }

        public async Task<bool> SaveVehiclesModel(Model.VehicleModel ObjVechicle)
        {
            try
            {

           
            using (var unitOfWork = new UnitOfWork(new VehicleDbEntities()))
            {
                unitOfWork.VehicleModels.Add(new Model.VehicleModel() { Id = objGen.UniqueId(), MakeId = ObjVechicle.MakeId, Name = ObjVechicle.Name, Abrv = ObjVechicle.Abrv });


                if (unitOfWork.Save() == 1)
                {
                    return await Task.FromResult(true);
                }
                else
                {
                    return await Task.FromResult(false);

                }
                }
            }
            catch (Exception ex)
            {
                //Logs can be stored into Database or can be Email to developer.

                objGen.ErrorLogging(ex, "SaveVehiclesModel");

                return false;
            }
        }

        public async Task<bool> UpdateVehicleModel(Model.VehicleModel ObjVechicle)
        {
            try
            {


                using (var unitOfWork = new UnitOfWork(new VehicleDbEntities()))
                {
                    var Obj = unitOfWork.VehicleModels.SingleOrDefault(s => s.Id.Equals(ObjVechicle.Id));
                    Obj.Name = ObjVechicle.Name;
                    Obj.Abrv = ObjVechicle.Abrv;

                    if (unitOfWork.Save() == 1)
                        return await Task.FromResult(true);
                    return await Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                //Logs can be stored into Database or can be Email to developer.

                objGen.ErrorLogging(ex, "UpdateVehicleModel");

                return false;
            }
        }

        public async Task<bool> DeleteVehicleModel(Model.VehicleModel ObjVechicle)
        {
            try
            {

           
            using (var unitOfWork = new UnitOfWork(new VehicleDbEntities()))
            {
                var VehicleModels = unitOfWork.VehicleModels.SingleOrDefault(s => s.Id.Equals(ObjVechicle.Id));
                unitOfWork.VehicleModels.Remove(VehicleModels);
                if (unitOfWork.Save() == 1)
                    return await Task.FromResult(true);
                return await Task.FromResult(false);
            }
            }
            catch (Exception ex)
            { 
                //Logs can be stored into Database or can be Email to developer.

                objGen.ErrorLogging(ex, "DeleteVehicleModel");

                return false;
            }
        }

       

    }
    }

