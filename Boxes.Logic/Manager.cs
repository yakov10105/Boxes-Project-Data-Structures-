using BinaryTree;
using Boxes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boxes.Logic
{
    public class Manager : IManager
    {
        #region Properties
        public ExpirationTester Tester { get; private set; }
        public BST<LengthData> LengthTree { get; private set; }
        public LinkedList<TimeData> ExpDateList { get; private set; }
        public double MaxDeviation { get; private set; } //Maximum deviation precentage
        public int MaxQtyPerSize { get; private set; }
        public int FittableBoxes { get; private set; } = 0; // total num of boxes that been found fittable 
        public bool IsBreak { get; private set; } = false; // stop condition for the buy method loop
        public List<HeightData> BoxesToBuy { get; private set; } = new List<HeightData>(); //references to the boxes that been found fittable (so the coumputer know which boxes to delete after users confirmation.)
        #endregion

        public Manager(double maxDeviation, int maxQtyPerSize, int maxGapInDays)
        {
            LengthTree = new BST<LengthData>();
            ExpDateList = new LinkedList<TimeData>();
            MaxDeviation = maxDeviation / 100.0;
            MaxQtyPerSize = maxQtyPerSize;
            Tester = new ExpirationTester(this, maxGapInDays);
        }

        #region IManager Methods

        #region Add
        public void AddBoxes(double width, double height, int QTY = 1)
        {
            int addedQTY = 0;
            //creating new objects with the given parameters
            Box box = new Box { Height = height, Length = width };
            TimeData timeData = new TimeData { Height = box.Height, Length = box.Length };
            LengthData lengthData = new LengthData { X = box.Length };
            HeightData heightData = new HeightData { Y = box.Height, TimeDataNode = new LinkedListNode<TimeData>(timeData), Father = lengthData };
            lengthData.SubTree.Add(heightData);

            BST<LengthData>.Node lengthNode = LengthTree.Find(lengthData); //checks if the tree already have node with the same x

            if (lengthNode != null)
            {
                BST<HeightData>.Node heightNode = lengthNode.data.SubTree.Find(heightData); //checks if the subtree already have node with the same y.
                if (heightNode != null)
                {
                    //Case 1 : X and Y exist                
                    CheckQtyAndAdd(timeData, heightNode, QTY - addedQTY, lengthNode);
                } //Case 1
                else
                {
                    //Case 2 : Only X exist
                    lengthNode.data.SubTree.Add(heightData);
                    AddBoxes(width, height, QTY - addedQTY);
                } //Case 2
            }
            else
            {
                //Case 3 : Either X and Y dont Exist
                LengthTree.Add(lengthData);
                heightData.Quantity++;

                timeData.FatherX = lengthData;
                timeData.FatherY = heightData;

                ExpDateList.AddLast(timeData);
                addedQTY++;
                if (addedQTY < QTY)
                {
                    AddBoxes(width, height, QTY - addedQTY);
                }
                heightData.TmpQuantity++;
            } //Case 3 

        }
        private void CheckQtyAndAdd(TimeData timeData, BST<HeightData>.Node heightNode, int qty, BST<LengthData>.Node lengthNode)
        {
            if (heightNode.data.Quantity + qty <= MaxQtyPerSize)
            {
                heightNode.data.Quantity += qty;
                heightNode.data.TmpQuantity += qty;
                heightNode.data.Father = lengthNode.data;
                timeData.FatherX = lengthNode.data;
                timeData.FatherY = heightNode.data;
            }
            else
            {
                heightNode.data.Quantity += (MaxQtyPerSize - heightNode.data.Quantity);
                heightNode.data.Father = lengthNode.data;
                timeData.FatherX = lengthNode.data;
                timeData.FatherY = heightNode.data;
                ExpDateList.AddLast(timeData);
                MessageBox.Show($"The current Box ({lengthNode.data.X}/{heightNode.data.Y}) have reached the MAX quantity in the stock\nThe stock was limited to : {MaxQtyPerSize} .\n" +
                    $"Numer of Added boxes: {(MaxQtyPerSize - heightNode.data.TmpQuantity)}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                heightNode.data.TmpQuantity += (MaxQtyPerSize - heightNode.data.Quantity);
            }
        }
        #endregion

        #region Show
        public BoxToShow ShowBox(double length, double height)
        {
            LengthData lengthData = new LengthData { X = length };
            HeightData heightData = new HeightData { Y = height };
            BST<LengthData>.Node lengthNode = LengthTree.Find(lengthData);
            if (lengthNode != null)
            {
                BST<HeightData>.Node heightNode = lengthNode.data.SubTree.Find(heightData);
                if (heightNode != null)
                {
                    if (heightNode.data.Quantity > 0)
                    {
                        Box b = new Box { Height = heightNode.data.Y, Length = lengthNode.data.X };
                        return new BoxToShow
                        {
                            Box = b,
                            AddedToStore = heightNode.data.TimeDataNode.Value.AddedToStoreDate,
                            LastDeal = heightNode.data.TimeDataNode.Value.LastDealDate,
                            Quantity = heightNode.data.Quantity
                        };
                    }
                }
                return null;
            }
            return null;
        }
        #endregion

        #region Buy&Delete
        public void FindBoxesToBuy(double length, double height, int QTY = 1)
        {
            while (FittableBoxes < QTY)
            {
                FindBoxes(ref length, height, QTY);
                if (IsBreak) break;
            }
            ShowBoughtBoxes(QTY);
        }
        private void FindBoxes(ref double length, double height, int QTY)
        {
            //creating new objects with the given parameters
            var lengthData = new LengthData { X = length };
            var heightData = new HeightData { Y = height };

            var lengthNode = LengthTree.Find(lengthData);//checks if the tree already have node with the same x
            if (lengthNode != null && lengthNode.data.IsAlreadyChecked == false)
            {
                var heightNode = lengthNode.data.SubTree.Find(heightData);//checks if the subtree already have node with the same y.
                if (heightNode != null && heightNode.data.IsAlreadyChecked == false)
                {
                    var currenQuantity = heightNode.data.Quantity;
                    if (currenQuantity - (QTY - FittableBoxes) >= 0)
                    {
                        //O(1)
                        heightNode.data.Quantity -= (QTY - FittableBoxes);
                        heightNode.data.QtyToDelete += (QTY - FittableBoxes);
                        FittableBoxes += (QTY - FittableBoxes);
                        heightNode.data.TimeDataNode.Value.LastDealDate = DateTime.Now;
                        BoxesToBuy.Add(heightNode.data);
                        heightNode.data.IsAlreadyChecked = true;
                    }
                    else
                    {
                        //O(1)
                        heightNode.data.Quantity -= currenQuantity;
                        heightNode.data.QtyToDelete += currenQuantity;
                        FittableBoxes += currenQuantity;
                        heightNode.data.TimeDataNode.Value.LastDealDate = DateTime.Now;
                        BoxesToBuy.Add(heightNode.data);
                        heightNode.data.IsAlreadyChecked = true;
                    }
                }  //Case 1
                else
                {
                    try
                    {
                        //Case 2 : No matching Y value was found - looking for another Y.
                        var newY = FindClosestY(lengthNode.data.SubTree.root, height);
                        FindBoxes(ref length, newY, QTY);
                        //lengthNode.data.IsAlreadyChecked = true;

                    }
                    //If also new Y value not found : looking for new X value.
                    catch (Exception)
                    {
                        lengthNode.data.IsAlreadyChecked = true;
                        try
                        {
                            var newX = FindClosestX(LengthTree.root, length);
                            FindBoxes(ref newX, height, QTY);
                            length = newX;
                            lengthNode.data.IsAlreadyChecked = true;
                        }
                        catch (Exception)
                        {
                            lengthNode.data.IsAlreadyChecked = true;
                            IsBreak = true;
                        }
                    }
                } //Case 2
            }
            else
            {
                //Case 3 : No matching X value was found - looking for another X.
                try
                {
                    var newX = FindClosestX(LengthTree.root, length);
                    FindBoxes(ref newX, height, QTY);
                    length = newX;
                    //lengthNode.data.IsAlreadyChecked = true;
                }
                catch (Exception)
                {
                    IsBreak = true;
                }
            } //Case 3
        }


        private double FindClosestX(BST<LengthData>.Node xNode, double x)
        {
            if (xNode == null) throw new Exception("No More fittable X values ...");
            var currentValue = xNode.data.X;
            if (currentValue >= x) // Searching in the left side of the tree
            {
                if (xNode.left == null && currentValue <= x + (x * MaxDeviation) && xNode.data.IsAlreadyChecked == false) return currentValue;
                if (xNode.left != null)
                {
                    var closest = FindClosestX(xNode.left, x);
                    if (closest >= x && (closest - x) <= (MaxDeviation * x) && (closest < currentValue) && closest >= x)
                    {
                        return closest;
                    }
                    //return currentValue;
                }
                if (xNode.right != null && xNode.right.data.IsAlreadyChecked == false)
                {
                    var closest = FindClosestX(xNode.right, x);
                    if (closest >= x && (closest - x) <= MaxDeviation * x && ((closest < currentValue)) && closest >= x)
                    {
                        return closest;
                    }
                    return currentValue;
                }
                else
                {
                    // return currentValue;
                    throw new Exception("No More fittable X values ...");
                }
            }
            else // Searching in the right side of the tree
            {
                if (xNode.right == null && currentValue <= x + (x * MaxDeviation) && currentValue > x && xNode.data.IsAlreadyChecked == false && currentValue >= x) return currentValue;
                if (xNode.right != null && xNode.right.data.IsAlreadyChecked == false)
                {
                    var closestR = FindClosestX(xNode.right, x);
                    if (closestR >= x && (closestR - x) <= MaxDeviation * x && /*(closestR < currentValue) &&*/ closestR >= x)
                    {
                        return closestR;
                    }
                    return currentValue;
                }
                if (xNode.left != null)
                {
                    var closestL = FindClosestX(xNode.left, x);
                    if (closestL >= x && (closestL - x) <= MaxDeviation * x && (closestL < currentValue) && closestL >= x)
                    {
                        return closestL;
                    }
                    return currentValue;
                }
                else
                {
                    return currentValue;
                    throw new Exception("No More fittable X values ...");
                }
            }
        }
        private double FindClosestY(BST<HeightData>.Node heightNode, double height)
        {
            if (heightNode == null) throw new Exception("new Y value was Not Found...");
            var currentValue = heightNode.data.Y;
            if (currentValue >= height) // Searching in the left side of the tree
            {
                if (heightNode.left == null && currentValue <= height + (height * MaxDeviation) && heightNode.data.IsAlreadyChecked == false) return currentValue;
                if (heightNode.left != null)
                {
                    var closest = FindClosestY(heightNode.left, height);
                    if (closest >= height && (closest - height) <= (MaxDeviation * height) && (closest < currentValue) && closest >= height)
                    {
                        return closest;
                    }
                    //return currentValue;
                }
                if (heightNode.right != null)
                {
                    var closest = FindClosestY(heightNode.right, height);
                    if (closest >= height && (closest - height) <= MaxDeviation * height && (closest > currentValue))
                    {
                        return closest;
                    }
                    return currentValue;
                }
                else
                {
                    throw new Exception("new Y value was not found ...");
                }
            }
            else // Searching in the right side of the tree
            {
                if (heightNode.right == null && currentValue >= height && currentValue <= height + (height * MaxDeviation)) return currentValue;
                if (heightNode.right != null)
                {
                    var closest = FindClosestY(heightNode.right, height);
                    if (closest >= height && (closest - height) <= MaxDeviation * height && /*(closest > currentValue)*/ closest >= height)
                    {
                        return closest;
                    }
                    return currentValue;
                }
                if (heightNode.left != null)
                {
                    var closest = FindClosestY(heightNode.left, height);
                    if (closest >= height && (closest - height) <= MaxDeviation * height && (closest < currentValue) && closest >= height)
                    {
                        return closest;
                    }
                    return currentValue;
                }
                else
                {
                    return currentValue;
                    //throw new Exception("new Y value was not found ...");
                }

            }
        }

        private void ShowBoughtBoxes(int QTY)
        {
            StringBuilder sb = new StringBuilder("Founded Boxes :\n");
            foreach (var i in BoxesToBuy)
            {
                sb.AppendLine($"Width: {i.Father.X} / Height: {i.Y} , Quantity: {i.QtyToDelete} ");
            }


            if (FittableBoxes < QTY)
            {
                var res = MessageBox.Show($"You wanted to buy {QTY} Boxes.\n{FittableBoxes} Boxes was found, You still want to buy the current amount ?\n {sb}",
                         "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                switch (res)
                {
                    case DialogResult.Yes:
                        DeleteSelectedBoxes();
                        break;
                    case DialogResult.No:
                        RestoreElements();
                        return;

                }
            }
            else
            {
                var res = MessageBox.Show($"{FittableBoxes} Boxes was found.\n{sb}\n Click 'Yes' to finish the progress.",
                        "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                switch (res)
                {
                    case DialogResult.Yes:
                        DeleteSelectedBoxes();
                        break;
                    case DialogResult.No:
                        RestoreElements();
                        return;

                }
            }
        }
        private void RestoreElements()
        {
            foreach (var i in BoxesToBuy)
            {
                i.Quantity = i.TmpQuantity;
                i.QtyToDelete = 0;
            }
            FittableBoxes = 0;
            LengthTree.ActInOrder(x => RestoreIsCheckedBool(x));
            IsBreak = false;
            BoxesToBuy.Clear();
        }
        private void DeleteSelectedBoxes()
        {
            foreach (var i in BoxesToBuy)
            {
                LengthData tmp = i.Father;
                i.Quantity = i.TmpQuantity - i.QtyToDelete;
                if (i.Quantity == 0)
                {
                    tmp.SubTree.Remove(i);
                    if (tmp.SubTree.IsRootNull())
                    {
                        LengthTree.Remove(tmp);
                    }
                }

            }
            FittableBoxes = 0;
            LengthTree.ActInOrder(x => RestoreIsCheckedBool(x));
            IsBreak = false;
            BoxesToBuy.Clear();
        }


        //Action for the ForEach method of the BST:
        private void RestoreIsCheckedBool(LengthData ld)
        {
            ld.IsAlreadyChecked = false;
            ld.SubTree.ActInOrder(x => x.IsAlreadyChecked = false);
        }
        #endregion

        public void CheckExpires()
        {
            Tester.Check();
            Tester.ShowDeletedItems();
        }

        #endregion

    }
}
