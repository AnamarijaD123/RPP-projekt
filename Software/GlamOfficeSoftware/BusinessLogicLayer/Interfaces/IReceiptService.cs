﻿using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IReceiptService
    {
        Task<IEnumerable<Receipt>> GetAllReceiptsAsync();
        Task AddNewReceiptAsync(Receipt receipt);
        Task<Receipt> VoidReceiptAsync(int receiptId, bool wantsGiftCardRecover = false);
        Task<IEnumerable<ReceiptDTO>> GetAllReceiptsDTOAsync();
        Task<string> GenerateReceiptInStringFormat(ReceiptDTO receiptDTO);
        Task GenerateReceiptPdf(ReceiptDTO receiptDTO);
        Task<IEnumerable<ReceiptDTO>> GetReceiptsByReceiptNumberPattrern(string receiptNumber);
        Task<IEnumerable<ReceiptDTO>> GetReceiptsByClientsFirstAndLastNamePattern(string firstAndLastNamePattern);
        Task<IEnumerable<ReceiptDTO>> GetReceiptsByEmployeesFirstAndLastNamePattern(string firstAndLastNamePattern);
    }
}
