﻿namespace Cryptaxation.Pdf.Contract
{
    public interface IK4FillLogic
    {
        void FillForms();
        void FillAndSaveForms();
        void FillForm();
        void FillDate();
        void FillNumbering();
        void FillName();
        void FillPersonalIdentificationNumber();
        void FillCurrencies();
        void FillResources();
    }
}