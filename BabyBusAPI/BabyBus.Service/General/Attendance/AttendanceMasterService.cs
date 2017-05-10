using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Attendance;
using BabyBus.Data.Repositories.Main;
using BabyBus.Data.Repositories.Relation;
using BabyBus.Model.Entities.Attendance;
using BabyBus.Service.Models;

namespace BabyBus.Service.General.Attendance {
    public interface IAttendanceService {
        IQueryable<AttendanceMaster> GetAllMaster();
        IQueryable<AttendanceDetail> GetAllDetail();
        void WorkAttendance(AttendanceModel model);
    }

    public class AttendanceService : IAttendanceService {
        private readonly IAttendanceMasterRepository attendanceMasterRepository;
        private readonly IAttendanceDetailRepository attendanceDetailRepository;
        private readonly IChildRepository childRepository;
        private readonly IParentChildRelationRepository pcrRepository;
        private readonly IUnitOfWork unitOfWork;

        public AttendanceService(IAttendanceMasterRepository attendanceMasterRepository, IUnitOfWork unitOfWork, IAttendanceDetailRepository attendanceDetailRepository, IChildRepository childRepository, IParentChildRelationRepository pcrRepository) {
            this.attendanceMasterRepository = attendanceMasterRepository;
            this.unitOfWork = unitOfWork;
            this.attendanceDetailRepository = attendanceDetailRepository;
            this.childRepository = childRepository;
            this.pcrRepository = pcrRepository;
        }

        public IQueryable<AttendanceMaster> GetAllMaster() {
            var items = attendanceMasterRepository.GetAll();
            return items;
        }

        public IQueryable<AttendanceDetail> GetAllDetail() {
            return attendanceDetailRepository.GetAll();
        }

        public void WorkAttendance(AttendanceModel model) {
            //Insert and has same data.
            if (model.MasterId == 0) {
                if (attendanceMasterRepository.GetAll().Any(x => x.CreateDate == model.CreateDate && x.ClassId == model.ClassId)) {
                    return;
                }
            }
            using (var scope = new TransactionScope()) {
                var attMaster = new AttendanceMaster();
                Mapper.DynamicMap(model, attMaster);

                if (attMaster.MasterId == 0) {
                    attendanceMasterRepository.Add(attMaster);
                    Save();
                }
                else {
                    attendanceMasterRepository.Update(attMaster);
                    Save();
                }
                //child
                var preChildren = (from c in attendanceDetailRepository.GetAll()
                                   where c.MasterId == attMaster.MasterId
                    select c.ChildId).ToList();

                var addList = model.AttChildren.Except(preChildren);
                var delList = preChildren.Except(model.AttChildren);
                foreach (var childId in addList) {
                    attendanceDetailRepository.Add(new AttendanceDetail {
                        ChildId = childId,
                        CreateDate = attMaster.CreateDate,
                        MasterId = attMaster.MasterId
                    });
                }
                foreach (var childId in delList) {
                    var id = childId;
                    attendanceDetailRepository.Delete(x=>x.ChildId==id);
                }
                Save();
                scope.Complete();
            }
        }

        public void Save() {
            unitOfWork.Commit();
        }
    }
}