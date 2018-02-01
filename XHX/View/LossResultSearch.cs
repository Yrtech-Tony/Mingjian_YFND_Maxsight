﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.DTO;
using XHX.Common;

namespace XHX.View
{
    public partial class LossResultSearch : BaseForm
    {
        XtraGridDataHandler<LossResultDto> dataHandler = null;
        localhost.Service webService = new localhost.Service();
        public LossResultSearch()
        {
            InitializeComponent();
            OnLoadView();
        }

        private void OnLoadView()
        {
            dataHandler = new XtraGridDataHandler<LossResultDto>(grvLossReg);
            InitData();
            
        }
        private void InitData()
        {
            //初始化查询条件
            BindComBox.BindProject(cboProject);
            btnShopCode.Text = String.Empty;
            txtShopName.Text = String.Empty;
            grcLossReg.DataSource = new List<LossResultDto>();
        }
        private void SearchLoss()
        {
            List<LossResultDto> LossList = new List<LossResultDto>();
            DataSet ds = webService.SearchLossResult(CommonHandler.GetComboBoxSelectedValue(cboProject).ToString(),btnShopCode.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    LossResultDto loss = new LossResultDto();
                    loss.ProjectCode = Convert.ToString(ds.Tables[0].Rows[i]["ProjectCode"]);
                    loss.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    loss.SubjectCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectCode"]);
                    loss.LossDesc = Convert.ToString(ds.Tables[0].Rows[i]["LossDesc"]);
                    loss.PicName = Convert.ToString(ds.Tables[0].Rows[i]["PicName"]);
                    loss.SeqNO = Convert.ToInt32(ds.Tables[0].Rows[i]["SeqNO"]);
                    LossList.Add(loss);
                }
            }

            grcLossReg.DataSource = LossList;
        }
        public override void InitButtonClick()
        {

            InitData();
        }
        public override void SearchButtonClick()
        {
            SearchLoss();
        }
        public override void ExcelDownButtonClick()
        {
            if (grvLossReg.DataSource != null)
                CommonHandler.ExcelExportByExporter(grvLossReg);
        }
        public override List<ButtonType> CreateButton()
        {
            List<ButtonType> list = new List<ButtonType>();
            list.Add(ButtonType.InitButton);
            list.Add(ButtonType.SearchButton);
            list.Add(ButtonType.ExcelDownButton);
            return list;
        }

        private void btnShopCode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Shop_Popup pop = new Shop_Popup("", "", false);
            pop.ShowDialog();
            ShopDto dto = pop.Shopdto;
            if (dto != null)
            {
                btnShopCode.Text = dto.ShopCode;
                txtShopName.Text = dto.ShopName;
            }
            else
            {
                btnShopCode.Text = "";
                txtShopName.Text = "";
            }
        }
    }
}
