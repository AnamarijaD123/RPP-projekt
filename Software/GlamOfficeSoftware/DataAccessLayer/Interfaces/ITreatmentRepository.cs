﻿using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ITreatmentRepository : IRepository<Treatment>
    {
        Task<IEnumerable<TreatmentDTO>> GetAllTreatmentsWithDetailsAsync();
        Task<IEnumerable<Treatment>> GetTreatmentsByNamePattern(string namePattern);
        Task<IEnumerable<TreatmentGroup>> GetAllTreatmentGroupsAsync();
        Task<IEnumerable<WorkPosition>> GetAllWorkPositionsAsync();
        Task<IEnumerable<TreatmentDTO>> GetTreatmentsByGroupAsync(int groupId);
        Task<IEnumerable<TreatmentDTO>> GetTreatmentsByWorkPositionAsync(int workPositionId);
    }
}
