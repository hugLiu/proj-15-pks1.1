using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTabbedMdi;
using DevExpress.LookAndFeel;

namespace DataGatherTool
{
    public partial class Main_Frame : XtraForm
    {
        public Main_Frame()
        {
            InitializeComponent();
            Load += Main_Frame_Load;
            //UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");
        }

        void Main_Frame_Load(object sender, EventArgs e)
        {
            tBtnReviewManager_ItemClick(null, null);
            barHeaderItem2.Caption = "";
        }
        /// <summary>
        /// 防止打开重复窗体
        /// </summary>
        /// <param name="p_ChildrenFormText"></param>
        /// <returns></returns>
        private bool ShowChildrenForm(string p_ChildrenFormText)
        {
            int i;
            //依次检测当前窗体的子窗体
            for (i = 0; i < this.MdiChildren.Length; i++)
            {
                //判断当前子窗体的Text属性值是否与传入的字符串值相同
                if (this.MdiChildren[i].Name == p_ChildrenFormText)
                {
                    //如果值相同则表示此子窗体为想要调用的子窗体，激活此子窗体并返回true值
                    this.MdiChildren[i].Activate();
                    barHeaderItem2.Caption = this.MdiChildren[i].Text;
                    return true;
                }
            }

            //如果没有相同的值则表示要调用的子窗体还没有被打开，返回false值
            return false;
        }

        private void nBtnObjTypeManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            bBtnObjTypeManager(null, null);
        }


        private void bBtnObjTypeManager(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (!ShowChildrenForm("ObjTypeManager"))
            //{
            //    ObjTypeManager frm = new ObjTypeManager();
            //    frm.MdiParent = this;
            //    frm.Show();
            //    barHeaderItem2.Caption = frm.Text;
            //}
        }

        private void nbBtnObjManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            bBtnObjManager(null, null);
        }

        private void bBtnObjManager(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (!ShowChildrenForm("ObjManager"))
            //{
            //    ObjManager frm = new ObjManager();
            //    frm.MdiParent = this;
            //    frm.Show();
            //    barHeaderItem2.Caption = frm.Text;
            //}
        }

        private void bBtnObjRelation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (!ShowChildrenForm("ObjRelationManager"))
            //{
            //    ObjRelationManager frm = new ObjRelationManager();
            //    frm.MdiParent = this;
            //    frm.Show();
            //    barHeaderItem2.Caption = frm.Text;
            //}
        }
        private void nBtnObjRelation_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            bBtnObjRelation_ItemClick(null, null);
        }
        private void tBtnReviewManager_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (!ShowChildrenForm("PReviewManager"))
            //{
            //    PReviewManager frm = new PReviewManager();
            //    frm.MdiParent = this;
            //    frm.Show();
            //    barHeaderItem2.Caption = frm.Text;
            //}
        }

        private void nBtnReviewManager_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            tBtnReviewManager_ItemClick(null, null);
        }

        private void tBtnEscSystem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void toolBtnObjTypeManager_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bBtnObjTypeManager(null, null);
        }

        private void toolBtnObjManager_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //bBtnObjManager(null, null);
            if (!ShowChildrenForm("ImpExcelData"))
            {
                ImpExcelData frm = new ImpExcelData();
                frm.MdiParent = this;
                frm.Show();
                barHeaderItem2.Caption = frm.Text;
            }

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //    if (!ShowChildrenForm("TypeClassManage"))
            //    {
            //        TypeClassManage frm = new TypeClassManage();
            //        frm.MdiParent = this;
            //        frm.Show();
            //        barHeaderItem2.Caption = frm.Text;
            //    }
        }

    }
}
