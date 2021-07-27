using Boxes.Models;
using System;
using System.Text;
using System.Windows.Forms;

namespace Boxes.Logic
{
    public class ExpirationTester
    {
        private readonly string _pre = "Deleted Boxes :\n";
        public Manager Manager { get;private set; }
        public int MaxGapInDays { get;private set; }
        public StringBuilder DeletedBoxes { get;private set; }

        public ExpirationTester(Manager manager, int maxGapInDays)
        {
            Manager = manager;
            MaxGapInDays = maxGapInDays;
            DeletedBoxes = new StringBuilder(_pre);
        }
        public void Check()
        {
            foreach (var i in Manager.ExpDateList)
            {
                if (i.LastDealDate != default && (DateTime.Now - i.LastDealDate).TotalDays < MaxGapInDays)
                {
                    break;
                }
                else if ((DateTime.Now - i.AddedToStoreDate).TotalDays < MaxGapInDays)
                {
                    break;
                }
                else
                {
                    Manager.ExpDateList.RemoveFirst();
                    Delete(i);
                    DeletedBoxes.AppendLine($"Width: {i.FatherX.X} Height: {i.FatherY.Y} Deleted Quantity: {i.FatherY.Quantity}");
                    Check();
                    break;
                }
            }
        }
        public void ShowDeletedItems()
        {
            if (DeletedBoxes.Length > _pre.Length)
                MessageBox.Show(DeletedBoxes.ToString(), "Expired Boxes :", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                return;
        }
        private void Delete(TimeData td)
        {
            td.FatherX.SubTree.Remove(td.FatherY);
            if (td.FatherX.SubTree.IsRootNull())
            {
                Manager.LengthTree.Remove(td.FatherX);
            }
        }
    }
}
