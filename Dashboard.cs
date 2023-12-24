using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace WindowsFormsApp1
{
    public partial class Dashboard : Form
    {
        private Dictionary<string, FoodItem> foodItems;
        private decimal subtotal = 0.0m;
        private decimal selectedPrice = 0.0m;
        private decimal totalPayment = 0.0m;
        private List<CartItem> cartItems;
        private int itemCount = 0; // Step 2

        public Dashboard()
        {
            InitializeComponent();
            InitializeFoodItems();
            InitializeCategoryComboBox();
            InitializeComboBox();
            InitializeQuantityComboBox();
            cartItems = new List<CartItem>();
        }

        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public class FoodItem
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        public class CartItem
        {
            private static int itemNumberCounter = 1;

            public int ItemNumber { get; }
            public string FoodName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }

            public decimal Total => Price * Quantity;

            public CartItem()
            {
                ItemNumber = itemNumberCounter++;
            }

            public override string ToString()
            {
                // Adjust the format for ListBoxCart
                return $"{ItemNumber,-3}. {FoodName,-35}";
            }

        }

        private void InitializeQuantityComboBox()
        {
            for (int i = 1; i <= 10; i++)
            {
                comboBoxQuantity.Items.Add(i);
            }

            // Set the default quantity
            comboBoxQuantity.SelectedIndex = 0;
        }

        private void InitializeCategoryComboBox()
        {
            // Populate the category ComboBox
            comboBoxCategory.Items.Add("Today's Offer");
            comboBoxCategory.Items.Add("Meals");
            comboBoxCategory.Items.Add("Sides");
            comboBoxCategory.Items.Add("Desserts");
            comboBoxCategory.Items.Add("Drinks");

            // Set the default category
            comboBoxCategory.SelectedIndex = 0; // Select "Meals"
        }
        private void InitializeComboBox()
        {
            // Populate the food items ComboBox based on the selected category
            string selectedCategory = comboBoxCategory.SelectedItem.ToString();

            foreach (var foodItem in foodItems.Values.Where(item => item.Category == selectedCategory))
            {
                comboBoxFood.Items.Add(foodItem);
            }
        }
        private void InitializeFoodItems()
        {
            // Initialize food items and their prices with categories
            foodItems = new Dictionary<string, FoodItem>
            {
                { "2 Cheeseburger + 2 Reg. French Fries", new FoodItem { Name = "2 Cheeseburger + 2 Reg. French Fries", Price = 120, Category = "Today's Offer" } },
                { "2 Cheeseburger + 2 Spicy Chicken", new FoodItem { Name = "2 Cheeseburger + 2 Spicy Chicken", Price = 140, Category = "Today's Offer" } },
                { "3 Crispy Chicken + 2 French Fries", new FoodItem { Name = "3 Crispy Chicken + 2 French Fries", Price = 160, Category = "Today's Offer" } },
                { "3 Cheeseburger + 3 Fruit Tea Lemon", new FoodItem { Name = "3 Cheeseburger + 3 Fruit Tea Lemon", Price = 120, Category = "Today's Offer" } },
                { "Large French Fries + Cola", new FoodItem { Name = "Large French Fries + Cola", Price = 70, Category = "Today's Offer" } },
                { "McNuggets 20 Pcs, Spicy", new FoodItem { Name = "McNuggets 20 Pcs, Spicy", Price = 240, Category = "Today's Offer" } },
                { "Blueberry Cheesecake Pie", new FoodItem { Name = "Blueberry Cheesecake Pie", Price = 34, Category = "Today's Offer" } },
                { "6 pcs Korean Soy Garlic Wings", new FoodItem { Name = "6 pcs Korean Soy Garlic Wings", Price = 100, Category = "Today's Offer" } },

                { "Chicken Happy Meal", new FoodItem { Name = "Chicken Happy Meal", Price = 90, Category = "Meals" } },
                { "4 Pcs McNuggets Happy Meal", new FoodItem { Name = "4 Pcs McNuggets Happy Meal", Price = 90, Category = "Meals" } },
                { "Beef Burger Happy Meal", new FoodItem { Name = "Beef Burger Happy Meal", Price = 90, Category = "Meals" } },
                { "Chicken Burger Happy Meal", new FoodItem { Name = "Chicken Burger Happy Meal", Price = 90, Category = "Meals" } },
                { "Big Mac", new FoodItem { Name = "Big Mac", Price = 82, Category = "Meals" } },
                { "Korean Soy Garlic Wings", new FoodItem { Name = "Korean Soy Garlic Wings", Price = 70, Category = "Meals" } },
                { "Fish Burger", new FoodItem { Name = "Fish Burger", Price = 64, Category = "Meals" } },
                { "9 Chicken McNuggets", new FoodItem { Name = "9 Chicken McNuggets", Price = 104, Category = "Meals" } },
                { "Triple Cheeseburger", new FoodItem { Name = "Triple Cheeseburger", Price = 110, Category = "Meals" } },

                { "Chicken Fingers", new FoodItem { Name = "Chicken Fingers", Price = 24, Category = "Sides" } },
                { "French Fries", new FoodItem { Name = "French Fries", Price = 40, Category = "Sides" } },
                { "Chicken Snack Wrap", new FoodItem { Name = "Chicken Snack Wrap", Price = 35, Category = "Sides" } },
                { "Spicy Chicken Bites", new FoodItem { Name = "Spicy Chicken Bites", Price = 26, Category = "Sides" } },
                { "Apple Pie", new FoodItem { Name = "Apple Pie", Price = 25, Category = "Sides" } },
                { "Butter Croissant", new FoodItem { Name = "Butter Croissant", Price = 50, Category = "Sides" } },
                { "Banana Muffin", new FoodItem { Name = "Banana Muffin", Price = 52, Category = "Sides" } },
                { "Choco Lover", new FoodItem { Name = "Choco Lover", Price = 72, Category = "Sides" } },
                { "Tuna Puff Pastry", new FoodItem { Name = "Tuna Puff Pastry", Price = 74, Category = "Sides" } },
                { "Cheese Stick", new FoodItem { Name = "Cheese Stick", Price = 50, Category = "Sides" } },

                { "Double Choco Sundae", new FoodItem { Name = "Double Choco Sundae", Price = 26, Category = "Desserts" } },
                { "Chocolate Strawberry Sundae", new FoodItem { Name = "Chocolate Strawberry Sundae", Price = 26, Category = "Desserts" } },
                { "Strawberry Sundae", new FoodItem { Name = "Strawberry Sundae", Price = 24, Category = "Desserts" } },
                { "Chocolate Sundae", new FoodItem { Name = "Chocolate Sundae", Price = 24, Category = "Desserts" } },
                { "Tiramisu McFlurry with Cookie Crumbs", new FoodItem { Name = "Tiramisu McFlurry with Cookie Crumbs", Price = 36, Category = "Desserts" } },
                { "Tiramisu McFlurry with Lotus Biscoff", new FoodItem { Name = "Tiramisu McFlurry with Lotus Biscoff", Price = 36, Category = "Desserts" } },
                { "McFlurry® featuring Oreo", new FoodItem { Name = "McFlurry® featuring Oreo", Price = 28, Category = "Desserts" } },
                { "Choco McFlurry®", new FoodItem { Name = "Choco McFlurry®", Price = 28, Category = "Desserts" } },

                { "Fanta McFloat", new FoodItem { Name = "Fanta McFloat", Price = 30, Category = "Drinks" } },
                { "Coke McFloat®", new FoodItem { Name = "Coke McFloat®", Price = 30, Category = "Drinks" } },
                { "Milo", new FoodItem { Name = "Milo", Price = 26, Category = "Drinks" } },
                { "Iced Coffee", new FoodItem { Name = "Iced Coffee", Price = 38, Category = "Drinks" } },
                { "Coca Cola®", new FoodItem { Name = "Coca Cola®", Price = 20, Category = "Drinks" } },
                { "Fanta®", new FoodItem { Name = "Fanta®", Price = 20, Category = "Drinks" } },
                { "Sprite®", new FoodItem { Name = "Sprite®", Price = 20, Category = "Drinks" } },
                { "Fruit Tea Lemon", new FoodItem { Name = "Fruit Tea Lemon", Price = 20, Category = "Drinks" } },
                { "Hot Tea", new FoodItem { Name = "Hot Tea", Price = 24, Category = "Drinks" } },
                { "Iced Coffee Matcha", new FoodItem { Name = "Iced Coffee Matcha", Price = 46, Category = "Drinks" } },

                // Add more food items as needed
            };
        }

        private void comboBoxFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            FoodItem selectedFood = (FoodItem)comboBoxFood.SelectedItem;
            selectedPrice = selectedFood.Price;

            label1.Text = selectedFood.Name;
            label2.Text = selectedPrice.ToString("0.00");
            label3.Text = "Quantity:";

            // Show the quantity ComboBox
            comboBoxQuantity.Visible = true;

        }

        private void buttonAddToCart_Click(object sender, EventArgs e)
        {
            if (comboBoxFood.SelectedItem != null)
            {
                FoodItem selectedFood = (FoodItem)comboBoxFood.SelectedItem;
                int quantity = (int)comboBoxQuantity.SelectedItem;

                CartItem cartItem = new CartItem
                {
                    FoodName = selectedFood.Name,
                    Price = selectedFood.Price,
                    Quantity = quantity
                };

                listBoxCart.Items.Add(cartItem);
                listBoxPrice.Items.Add($"${selectedFood.Price}");
                listBoxQty.Items.Add($"({quantity})");
                listBoxTotal.Items.Add($"${cartItem.Total}");

                subtotal += cartItem.Total;
                itemCount++; // Step 3
                UpdateSubtotal();
            }
        }
        private void UpdateSubtotal()
        {
            label7.Text = subtotal.ToString("0.00");
            label34.Text = itemCount.ToString(); // Step 5

            // Refresh the listboxes to update the display
            listBoxCart.Refresh();
            listBoxPrice.Refresh();
            listBoxQty.Refresh();
            listBoxTotal.Refresh();
        }

        private void buttonCheckout_Click(object sender, EventArgs e)
        {
            // Check which order type is selected
            string orderType = radioButtonDelivery.Checked ? "Delivery" : "Pickup";

            // Get the address if delivery is selected
            string address = radioButtonDelivery.Checked ? textBoxAddress.Text : "N/A";

            // Now you can use the 'orderType' and 'address' variables in your checkout logic
            tabControl1.SelectedTab = tabPage7;
            textBoxUsername.Text = Username;
            textBoxPhoneNumber.Text = PhoneNumber;
            label13.Text = orderType;
            label14.Text = address;

            // Update payment summary when checking out
            UpdatePaymentSummary();

        }

        private void UpdatePaymentSummary()
        {
            // Update labels with the latest payment information
            label27.Text = subtotal.ToString("0.00");

            // Check if the order type is delivery to decide whether to include delivery fee
            if (radioButtonDelivery.Checked)
            {
                // Fixed delivery fee for delivery orders
                decimal deliveryFee = 30.0m;
                label28.Text = deliveryFee.ToString("0.00");
            }
            else
            {
                label28.Text = "Pick-Up";
            }

            // Calculate other fees based on the number of items in the cart
            decimal otherFees = CalculateOtherFees();
            label29.Text = otherFees.ToString("0.00");

            // Calculate total payment
            decimal totalPayment = subtotal + (radioButtonDelivery.Checked ? 30.0m : 0.0m) + otherFees;
            label31.Text = totalPayment.ToString("0.00");
        }
        private decimal CalculateOtherFees()
        {
            // Calculate other fees based on the number of items in the cart
            int numberOfItems = listBoxCart.Items.Count;

            // Base fee
            decimal baseFee = 5.0m;

            // Additional fee for more than 5 items
            decimal additionalFee = numberOfItems > 5 ? 5.0m : 0.0m;

            return baseFee + additionalFee;
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxFood.Items.Clear();

            // Populate the food items ComboBox based on the selected category
            string selectedCategory = comboBoxCategory.SelectedItem.ToString();

            foreach (var foodItem in foodItems.Values.Where(item => item.Category == selectedCategory))
            {
                comboBoxFood.Items.Add(foodItem);
            }
        }

        private void radioButtonDelivery_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDelivery.Checked)
            {
                // Delivery is selected
                // Enable the address textbox for input
                textBoxAddress.Enabled = true;
            }
            else
            {
                // Delivery is not selected
                // Disable the address textbox
                textBoxAddress.Enabled = false;
                // Optionally, you can clear the textbox content
                textBoxAddress.Text = string.Empty;
            }

        }

        private void radioButtonPickup_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAddress.Enabled = false;
            // Optionally, you can clear the textbox content
            textBoxAddress.Text = string.Empty;
        }

        private void buttonPaymentOptions_Click(object sender, EventArgs e)
        {
            label32.Text = label31.Text;
            tabControl1.SelectedTab = tabPage8;
        }

        private void pictureBoxCash_Click(object sender, EventArgs e)
        {
            labelThankYou.Visible = true;
            labelOrderAgain.Visible = true;
            buttonYes.Visible = true;
            buttonNo.Visible = true;
            
        }

        private void pictureBoxLinePay_Click(object sender, EventArgs e)
        {
            labelThankYou.Visible = true;
            labelOrderAgain.Visible = true;
            buttonYes.Visible = true;
            buttonNo.Visible = true;
            
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            ResetInputValues();
            tabControl1.SelectedTab = tabPage1; // Chang
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void ResetInputValues()
        {
            listBoxCart.Items.Clear();
            listBoxPrice.Items.Clear();
            listBoxQty.Items.Clear();
            listBoxTotal.Items.Clear();
            subtotal = 0.0m;
            itemCount = 0; // Step 4
            UpdateSubtotal();
            // Clear other input fields and reset values
            textBoxAddress.Text = string.Empty;
            radioButtonDelivery.Checked = false;
            radioButtonPickup.Checked = false;

            // Disable the address textbox
            textBoxAddress.Enabled = false;

            // Clear order type labels
            label13.Text = "";
            label14.Text = "";

            // Clear payment summary labels
            label27.Text = "0.00";
            label28.Text = "0.00";
            label29.Text = "0.00";
            label31.Text = "0.00";

            // Clear the thank-you message and buttons
            labelThankYou.Visible = false;
            labelOrderAgain.Visible = false;
            buttonYes.Visible = false;
            buttonNo.Visible = false;
            

        }

     
    }


}