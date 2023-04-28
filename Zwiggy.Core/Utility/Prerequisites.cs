using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Zwiggy.Core.Utility
{
    public class Prerequisites
    {
        public static void PopulateDatabase()
        {
            if (!File.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path, "swiggy.db")))
            {
                using (SqliteConnection connection = new SqliteConnection($"Data Source={Path.Combine(ApplicationData.Current.LocalFolder.Path, "swiggy.db")}"))
                {
                    connection.Open();

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        // CREATING DB TABLES
                        command.CommandText = "CREATE TABLE users (email TEXT NOT NULL UNIQUE,name TEXT NOT NULL,password TEXT NOT NULL,PRIMARY KEY(email));";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE restaurants (id TEXT NOT NULL UNIQUE,name TEXT NOT NULL,latitude TEXT NOT NULL,longitude TEXT NOT NULL,costForTwo INTEGER NOT NULL,imagePath TEXT NOT NULL,PRIMARY KEY(id)); ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE cuisineCategories (id TEXT NOT NULL UNIQUE,category TEXT,PRIMARY KEY(id)); ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE cuisines (cuisineCategoryId TEXT NOT NULL,restaurantId TEXT NOT NULL,FOREIGN KEY(restaurantId) REFERENCES restaurants(id)); ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE cart (dishId TEXT NOT NULL,dishCount INTEGER NOT NULL,userId TEXT NOT NULL,FOREIGN KEY(dishId) REFERENCES dishes(id),FOREIGN KEY(userId) REFERENCES users(email));";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE dishCategories (id TEXT NOT NULL UNIQUE,category TEXT,PRIMARY KEY(id)); ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE dishes (id TEXT NOT NULL UNIQUE,name TEXT NOT NULL,cost REAL NOT NULL,description TEXT NOT NULL,isVeg INTEGER NOT NULL,soldCount INTEGER NOT NULL,imagePath TEXT NOT NULL,dishCategoryId TEXT NOT NULL,restaurantId TEXT NOT NULL,PRIMARY KEY(id),FOREIGN KEY(dishCategoryId) REFERENCES dishCategories(id),FOREIGN KEY(restaurantId) REFERENCES restaurants(id)); ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE favouriteRestaurants (userId TEXT NOT NULL,restaurantId  TEXT NOT NULL,PRIMARY KEY(userId,restaurantId),FOREIGN KEY(userId) REFERENCES users(email),FOREIGN KEY(restaurantId) REFERENCES restaurants(id));";
                        command.ExecuteNonQuery();
                        
                        command.CommandText = "CREATE TABLE address (id TEXT NOT NULL,addressDescription TEXT NOT NULL,nickname TEXT NOT NULL,userId TEXT NOT NULL,PRIMARY KEY(id),FOREIGN KEY(userId) REFERENCES users(email));";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE orderItems (orderId TEXT NOT NULL,dishId TEXT NOT NULL,dishCount INTEGER NOT NULL,userId TEXT NOT NULL,FOREIGN KEY(orderId) REFERENCES orders(id),FOREIGN KEY(dishId) REFERENCES dishes(id),FOREIGN KEY(userId) REFERENCES users(email));";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE orders (id TEXT NOT NULL,restaurantId TEXT NOT NULL,addressId TEXT NOT NULL,modeOfPayment NUMBER NOT NULL,userId TEXT NOT NULL,orderPlacedDateAndTime TEXT NOT NULL,isDelivered INTEGER NOT NULL,PRIMARY KEY(id),FOREIGN KEY(addressId) REFERENCES address(id),FOREIGN KEY(userId) REFERENCES users(email));";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO users VALUES('','','');";
                        command.ExecuteNonQuery();

                        // LIST OF INPUTS

                        string[] restaurantName = {"Ambur Star Biriyani",
                            "Zaitoon",
                            "Wow! Momo",
                            "Burger King",
                            "Faasos - Wraps & Rolls",
                            "Dinidigul Thalappakatti",
                            "Hot Chips",
                            "Domino's Pizza",
                            "Perambur Sri Srinivasa",
                            "SS Hyderabad Biriyani",
                            "Hotel Saravana Bhavan",
                            "KFC",
                            "Sangeetha",
                            "McDonald's",
                            "The Red Box",
                            "Pizza Hut",
                            "Subway",
                            "Anjappar",
                            "Mama's Little Bakery",
                            "Hotel Buhari",
                            "Cream & Fudge",
                            "Firangi Bake",
                            "Frozen Bottle",
                            "The Good Bowl",
                            "Khalids Biriyani",
                            "Junior Kuppanna",
                            "LunchBox",
                            "Hotel Mathuraa",
                            "On de' Rocks",
                            "Savoury Sea Shell",
                            "Sweet Truth"};

                        int[] costForTwo = { 500,
                            900,
                            300,
                            350,
                            200,
                            400,
                            250,
                            400,
                            300,
                            300,
                            300,
                            400,
                            300,
                            400,
                            400,
                            300,
                            350,
                            400,
                            300,
                            400,
                            200,
                            400,
                            250,
                            400,
                            350,
                            450,
                            200,
                            200,
                            300,
                            500,
                            450
                        };

                        string[] restaurantImagePath = { "../../Assets/Images/Restaurant/ambur.png",
                            "../../Assets/Images/Restaurant/zaitoon.png",
                            "../../Assets/Images/Restaurant/wowMomo.png",
                            "../../Assets/Images/Restaurant/burgerKing.png",
                            "../../Assets/Images/Restaurant/faasos.png",
                            "../../Assets/Images/Restaurant/thalapakatti.png",
                            "../../Assets/Images/Restaurant/hotChips.png",
                            "../../Assets/Images/Restaurant/dominos.png",
                            "../../Assets/Images/Restaurant/srinivasa.png",
                            "../../Assets/Images/Restaurant/hyderabadi.png",
                            "../../Assets/Images/Restaurant/saravana.png",
                            "../../Assets/Images/Restaurant/kfc.png",
                            "../../Assets/Images/Restaurant/sangeetha.png",
                            "../../Assets/Images/Restaurant/mcD.png",
                            "../../Assets/Images/Restaurant/redbox.png",
                            "../../Assets/Images/Restaurant/pizzaHut.png",
                            "../../Assets/Images/Restaurant/subway.png",
                            "../../Assets/Images/Restaurant/anjappar.png",
                            "../../Assets/Images/Restaurant/bakery.png",
                            "../../Assets/Images/Restaurant/buhari.png",
                            "../../Assets/Images/Restaurant/creamAndFudge.png",
                            "../../Assets/Images/Restaurant/firangiBake.png",
                            "../../Assets/Images/Restaurant/frozenBottle.png",
                            "../../Assets/Images/Restaurant/goodBowl.png",
                            "../../Assets/Images/Restaurant/khalidsBiriyani.png",
                            "../../Assets/Images/Restaurant/kuppanna.png",
                            "../../Assets/Images/Restaurant/lunchBox.png",
                            "../../Assets/Images/Restaurant/mathuraa.png",
                            "../../Assets/Images/Restaurant/rocks.png",
                            "../../Assets/Images/Restaurant/seaShell.png",
                            "../../Assets/Images/Restaurant/sweetTruth.png",
                        };

                        string[] cuisines = { "Biriyani",
                            "Arabian",
                            "Kebab",
                            "BBQ",
                            "North Indian",
                            "Seafood",
                            "Momos",
                            "Tibetan",
                            "Burgers",
                            "Fast Food",
                            "Beverages",
                            "Rolls",
                            "Wraps",
                            "Desserts",
                            "South Indian",
                            "Sichuan",
                            "Chinese",
                            "Mithai",
                            "Lebanese",
                            "Street Food",
                            "Thai",
                            "Chettinad",
                            "Mughlai",
                            "Ice Cream",
                            "Pizzas",
                            "Italian",
                            "Pastas",
                            "Salads",
                            "Snacks",
                            "Bakery",
                            "Mexican",
                            "Waffle",
                            "Punjabi",
                            "Tandoor",
                            "Kerala",
                            "Indian"
                        };

                        List<List<int>> restaurantCuisines = new List<List<int>>{
                            new List<int> {1,17,5,23,16},
                            new List<int> {2,3,4,5,6},
                            new List<int> {7,8},
                            new List<int> {9,10,11},
                            new List<int> {12,10,13,14},
                            new List<int> {1,15,6,5,16,11,17},
                            new List<int> {15,5,20,17,11},
                            new List<int> {25,26,27,14},
                            new List<int> {18,20,10,11},
                            new List<int> {1,17,5,19,6},
                            new List<int> {15,5,17,20,24,11},
                            new List<int> {9,10,1,14,11},
                            new List<int> {15,5},
                            new List<int> {9,10},
                            new List<int> {7,17,16,21},
                            new List<int> {25,10},
                            new List<int> {28,29,14,11},
                            new List<int> {22,15,1,17},
                            new List<int> {14,30,29},
                            new List<int> {1,5,17},
                            new List<int> {24,14,11},
                            new List<int> {26,31,27,24},
                            new List<int> {11,14,24,32},
                            new List<int> {5,33,1,17},
                            new List<int> {1},
                            new List<int> {1,15,17,34},
                            new List<int> {5,33,1},
                            new List<int> {5,15,17},
                            new List<int> {10,29,25},
                            new List<int> {2,35,24,1,36},
                            new List<int> {30,14}
                        };

                        String[] dishCategories = {"Biriyani",
                            "Soup",
                            "Starters",
                            "Desserts",
                            "Main Course",
                            "Momos",
                            "Beverages",
                            "Burgers",
                            "Wraps",
                            "Sides",
                            "Rice Bowls",
                            "Sides",
                            "Combos",
                            "Tiffin",
                            "Chips",
                            "Snacks",
                            "Sweets",
                            "Halal Fried Chicken",
                            "Dips",
                            "Lunch",
                            "Snacks",
                            "Veg Pizza",
                            "Non-Veg Pizza",
                            "Sandwiches",
                            "Salads",
                            "Cakes",
                            "Scoops",
                            "Milkshake",
                            "Pastries",
                            "Waffle",
                            "Lasagne",
                            "Quesadilla",
                            "Thickshakes",
                            "Ice-Creams",
                            "Bucket Biriyani",
                            "LunchBox",
                            "Breads",
                            "Pizza",
                            "Cheesecakes"
                        };

                        List<List<ArrayList>> dishes = new List<List<ArrayList>>
                        {
                            new List<ArrayList>
                            {
                                new ArrayList{"Chicken Biriyani",285, "Delicious and savory Ambur Biryani cooked with seeraga samba rice, loaded with spicy marinated chicken. Serverd with brinjal and raita.",0, 255,"../../Assets/Images/Dish/chickenBiriyani.png",1,1},
                                new ArrayList{"Mutton Biriyani",385, "Delicious and savory Ambur Biryani cooked with seeraga samba rice, loaded with spicy marinated mutton. Serverd with brinjal and raita.",0, 400, "../../Assets/Images/Dish/muttonBiriyani.png", 1,1},
                                new ArrayList{"Egg Biriyani",255, "Delicious savory Ambur biryani cooked with Seeraga samba rice served with egg, brinjal and raita",0, 90, "../../Assets/Images/Dish/eggBiriyani.png", 1,1},
                                new ArrayList{"Veg Clear Soup",125, "A simple soup of assorted vegetables cooked in a lightly flavoured stock.",1, 400, "../../Assets/Images/Dish/vegClearSoup.png", 2,1},
                                new ArrayList{"Chicken Clear Soup",155, "A delectable soup prepared by simmering chicken slivers in a delightful hot and sour stock",0, 500, "../../Assets/Images/Dish/vegClearSoup.png", 2,1},
                                new ArrayList{"Paneer 65",220, "Deep-fried Paneer, marinated in a variety of spices and cooked to perfection.",1, 220, "../../Assets/Images/Dish/paneer65.png", 3,1},
                                new ArrayList{"Chicken 65",290, "Deep-fried boneless chicken pieces cooked in special sauce with green chilli and curry leaves",0, 300, "../../Assets/Images/Dish/chicken65.png", 3,1},
                                new ArrayList{"Chicken Lollipop",280, "Everyones favorite spiced chicken lollipop dipped in a flavorful batter and deep fried to perfection.",0, 180, "../../Assets/Images/Dish/chickenLollipop.png", 3,1},
                                new ArrayList{"Gulab Jamun",90, "Gulab Jamun. Soft. Plump and the way we are used to. There is nothing else in our gulab jamuns than authenticity and love.",1, 120, "../../Assets/Images/Dish/gulabJamun.png", 4,1},
                                new ArrayList{"Bread Halwa",70, "An Indian sweet made with bread cooked in ghee",1, 150, "../../Assets/Images/Dish/breadHalwa.png",4,1},
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Veg Clear Soup", 170, "A simple soup of assorted vegetables cooked in a lightly flavoured stock.",1, 200, "../../Assets/Images/Dish/vegClearSoup.png", 2, 2},
                                new ArrayList{"Chicken Clear Soup", 190, "A delectable soup prepared by simmering chicken slivers in a delightful hot and sour stock",0, 300, "../../Assets/Images/Dish/vegClearSoup.png", 2, 2},
                                new ArrayList{"Paneer 65",320, "Deep-fried Paneer, marinated in a variety of spices and cooked to perfection.",1, 180, "../../Assets/Images/Dish/paneer65.png", 3,2},
                                new ArrayList{"Chicken 65",290, "Chicken 65 is a popular deep fried spicy chicken appetizer served with fried curry leaves roundels onion and lemon wages",0, 260, "../../Assets/Images/Dish/chicken65.png", 3,2},
                                new ArrayList{"Prawn 65",470, "Everyones favorite spiced Prawn dipped in a flavorful batter and deep fried to perfection.",0, 300, "../../Assets/Images/Dish/prawn65.png", 3,2},
                                new ArrayList{"Chicken Biriyani",345, "Richly flavored aromatic rice layered with succulent chicken in a delicate blend of whole spices; served along with raita and curry.",0, 205, "../../Assets/Images/Dish/chickenBiriyani.png", 5,2},
                                new ArrayList{"Mutton Biriyani",345, "Richly flavored aromatic rice layered with succulent mutton in a delicate blend of whole spices; served along with raita and curry.",0, 408, "../../Assets/Images/Dish/muttonBiriyani.png", 5,2},
                                new ArrayList{"Egg Biriyani",255, "Healthy yet wholesome boiled eggs covered in flavor-packed masala and slow cooked rice.",0, 90, "../../Assets/Images/Dish/eggBiriyani.png", 5,2},
                                new ArrayList{"Chicken Fried Rice",240, "A slightly spicy dish made by tossing juicy chicken and rice in a flavorful sauce.",0, 112, "../../Assets/Images/Dish/friedRice.png", 5,2},
                                new ArrayList{"Gulab Jamun",180, "Gulab Jamun. Soft. Plump and the way we are used to. There is nothing else in our gulab jamuns than authenticity and love.",1, 80, "../../Assets/Images/Dish/gulabJamun.png", 4,2},
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Chicken Cheese Steam Momo",229, "An all-time favourite, Mouth-watering Momos stuffed with mix of Juicy boneless Chicken, Cheese, Coriander, Herbs and Indian Spices. Served with Spicy Red Sauce and Green Chutney.",0, 300, "../../Assets/Images/Dish/momo.png", 6,3},
                                new ArrayList{"Chicken Masala Steam Momo",199, "Soft juicy boneless chicken, with the flavour of in house Spices. Served with Schezwan spicy red sauce and green coriander Chutney.",0, 200, "../../Assets/Images/Dish/momo.png", 6,3},
                                new ArrayList{"Veggie Classic Steam Momo",129, "Stuffed with freshly chopped Vegetable's and mixed with the flavour of indian spices. Served with home-made spicy red sauce and We do not serve green coriander sauce.",1, 180, "../../Assets/Images/Dish/momo.png", 6,3},
                                new ArrayList{"Chicken Cheese Fried Momo",269, "Filled with juicy chicken and shredded cheese and mixed with the flavours of Indian masala. To make it crispy dipped in hot oil and served with red and green sauce.",0, 420, "../../Assets/Images/Dish/momo.png", 6,3},
                                new ArrayList{"Veggie Classic Fried Momo",169, "From our house of Veggie Delight: Filled with freshly chopped Vegetables and mixed with Indian Masala. Crispy Fried till it reaches golden brown in colour. Served with Red and We do not serve Green sauce.",1, 110, "../../Assets/Images/Dish/momo.png", 6,3},
                                new ArrayList{"Chicken Masala Pan Fried Momo in Schezwan Sauce",259, "Hot and crispy fried chicken masala momo tossed in Schezwan sauce [Spicy] and garnished with coriander We don't serve additional Sauce with this plate of Momos.",0, 360, "../../Assets/Images/Dish/momo.png", 6,3},
                                new ArrayList{"Red Bull",166.67, "A Soft Drink.",1, 200, "../../Assets/Images/Dish/redbull.png", 7,3},
                                new ArrayList{"Pepsi",57.14,"A Soft Drink.",1,100, "../../Assets/Images/Dish/pepsi.png", 7,3},
                                new ArrayList{"7UP",57.14,"A Soft Drink",1,50, "../../Assets/Images/Dish/7up.png", 7,3},
                                new ArrayList{"Coolberg",103.81, "A unique twist to a berry brew , Coolberg Cranberry Zero Alcohol Beer is everyone's delight.",1, 111, "../../Assets/Images/Dish/coolberg.png", 7,3}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Crispy Veg Burger",70, "Our best-selling burger with crispy veg patty, fresh onion and our signature sauce",1, 120, "../../Assets/Images/Dish/burger.png", 8,4},
                                new ArrayList{"Crispy Chicken Burger",90, "Our best-selling burger with crispy chicken patty, onion and classic mayo sauce.",0, 360, "../../Assets/Images/Dish/burger.png", 8,4},
                                new ArrayList{"Tikki Twist Burger",70, "Tikki bhi, Twist bhi! Our new signature burger with spicy sauce, onion, mix veg patty & a crunchy twist!",1, 200, "../../Assets/Images/Dish/burger.png", 8,4},
                                new ArrayList{"Chicken Makhani Burst Burger",99, "Enjoy rich makhani flavour with crispy chicken patty topped with fresh onion",0, 180, "../../Assets/Images/Dish/burger.png", 8,4},
                                new ArrayList{"Crunchy Chicken Taco",109, "Crispy Taco Filled With Chicken & Creamy Sauce",0, 400, "../../Assets/Images/Dish/taco.png", 9,4},
                                new ArrayList{"Chicken Fries",79, "The perfect crispy partner.",0, 300, "../../Assets/Images/Dish/fries.png", 10,4},
                                new ArrayList{"Peri Peri Fries",134, "Crispy fries with peri peri spice. Hot indeed",1, 111, "../../Assets/Images/Dish/fries.png", 10,4},
                                new ArrayList{"Hashbrown",39, "What every potato wants to be.",1, 211, "../../Assets/Images/Dish/hashbrowns.png", 10,4},
                                new ArrayList{"Veggie Strips",49, "Vegetarian's favourite finger food.",1, 420, "../../Assets/Images/Dish/strips.png", 10,4},
                                new ArrayList{"Pepsi",89, "A Soft Drink.",1, 50, "../../Assets/Images/Dish/pepsi.png", 7,4}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Cheese Melt Chicken Bhuna Wrap",205, "Craving for something new? We got your back. We mean, we got you a wrap. Chunks of chicken cooked in bhuna masala, topped with sinful cheese into a soft roti.",0, 360, "../../Assets/Images/Dish/wrap.png", 9,5},
                                new ArrayList{"Cheese Melt Chicken Tikka Wrap",205, "Craving for something new? We got your back. We mean, we got you a wrap. Chunks of chicken cooked in tikka masala, topped with sinful cheese into a soft roti.",0, 300, "../../Assets/Images/Dish/wrap.png", 9,5},
                                new ArrayList{"Masala Paneer Tikka Wrap",175, "Fresh paneer is smoked to perfection & drizzled with minty, spicy mayonnaise & wrapped in soft roti.",1, 111, "../../Assets/Images/Dish/wrap.png", 9,5},
                                new ArrayList{"Smoked Butter Chicken Rice Bowl",255, "Everyone loves a good butter chicken and this one is made with added excitement to surprise your tastebuds! Served over a bed of Spicy Hyderabadi Rice or Classic Flavourful Rice",0, 255, "../../Assets/Images/Dish/riceBowl.png", 11,5},
                                new ArrayList{"Paneer Signature Rice Bowl",229, "Let your taste buds fall in love with this unique bowl made using out-of-the-world paneer tikka chunks served over a bed of Spicy Hyderabadi Rice or Classic Flavourful Rice",1, 229, "../../Assets/Images/Dish/riceBowl.png", 11,5},
                                new ArrayList{"French Fries",109, "Just one can never be enough! Golden fried crunchy Fries that are salted to perfection.",1, 109, "../../Assets/Images/Dish/fries.png", 10,5},
                                new ArrayList{"Cheesy Chicken Meatballs",129, "Juicy minced chicken meatballs, served with a cheesy mayonnaise dip - A no-brainer for snack cravings!",0, 129, "../../Assets/Images/Dish/meatballs.png", 10,5},
                                new ArrayList{"Potato Wedges",99, "When you dont know what to eat next we suggest keep it simple and call for the wedges.",1, 99, "../../Assets/Images/Dish/wedges.png", 10,5},
                                new ArrayList{"Coca - Cola",57, "Coca-Cola Bottle.",1, 155, "../../Assets/Images/Dish/cocaCola.png", 7,5},
                                new ArrayList{"Swig",59, "Aerated drink flavored with the mouth-puckering green apple. Sure to refresh you completely!",1, 55, "../../Assets/Images/Dish/swig.png", 7,5},
                                new ArrayList{"Cheese Cake",179, " This New York Style Mango Cheesecake, made with premium quality cream cheese and a crust infused with delicious mango flavor, is every dessert lovers delight!",1, 179, "../../Assets/Images/Dish/cheeseCake.png", 4,5},
                                new ArrayList{"Brownie",119, "Gooey & fudgy on the inside and nutty on the outside. Our melt-in-the-mouth Single Nutty Hazelnut Brownie will give you serious dessert goals.",1, 119, "../../Assets/Images/Dish/brownie.png", 4,5}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Chilli Chicken",299, "A sweet, spicy and slightly sour dish made with diced chicken, onions and bell peppers",0, 302, "../../Assets/Images/Dish/chilliChicken.png", 3,6},
                                new ArrayList{"Mutton Liver Roast",279, "Butter soft mutton liver, dry roasted with a secret \"Curry Patta\" masala",0, 172, "../../Assets/Images/Dish/muttonLiver.png", 3,6},
                                new ArrayList{"Chicken Manchurian",279, "Deep-fried chicken sauteed with onions, capsicums and tossed with soy and chili sauce",0, 222, "../../Assets/Images/Dish/chickenManchurian.png", 3,6},
                                new ArrayList{"Chicken Biriyani",279, "Our age-old classic seeraga samba chicken Biryani with hand-ground spices from the foothills of Dindigul - Served With Boiled egg, Raita & Salna.",0, 502, "../../Assets/Images/Dish/chickenBiriyani.png", 1,6},
                                new ArrayList{"Mutton Biriyani",329, "Organic seeraga samba rice cooked with succulent mutton pieces of baby goat meat and hand-grounded spices.",0, 777, "../../Assets/Images/Dish/muttonBiriyani.png", 1,6},
                                new ArrayList{"Egg Biriyani",239, "Organic seeraga samba thalappakatti Biryani with boiled eggs of Dindigul - Served With Raita & Salna.",0, 239, "../../Assets/Images/Dish/eggBiriyani.png", 1,6},
                                new ArrayList{"Chicken Bucket Biriyani",1559, "4 to 5 portions of Thalappakatti Chicken Biryani served with 1 portion of Chicken 65, 4 boiled eggs, Raita & Gravy",0, 303, "../../Assets/Images/Dish/chickenBiriyani.png", 13,6},
                                new ArrayList{"Mutton Bucket Biriyani",1879, "4 to 5 portions of Thalappakatti Mutton Biryani served with 1 portion of Chicken 65, 4 boiled eggs, Raita & Gravy",0, 363, "../../Assets/Images/Dish/muttonBiriyani.png", 13,6},
                                new ArrayList{"Coke",36.28, "A Soft Drink.",1, 202, "../../Assets/Images/Dish/cocaCola.png", 7,6},
                                new ArrayList{"Thumbs Up",36.28, "A Soft Drink.",1, 52, "../../Assets/Images/Dish/thumbsUp.png", 7,6},
                                new ArrayList{"Sprite",36.28, "A Soft Drink.",1, 201, "../../Assets/Images/Dish/sprite.png", 7,6}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Chapathi Kuruma",80, "Thin whole wheat based Indian flatbread",1, 102, "../../Assets/Images/Dish/chapathi.png", 14,7},
                                new ArrayList{"Parotta Kuruma",80, "Layered and flaky flatbread roasted with little oil",1, 98, "../../Assets/Images/Dish/parotta.png", 14,7},
                                new ArrayList{"Poori Masala",80, "A delicious Indian breakfast dish comprising of 2 big circular fluffy deep fried wheat flour Pooris served with a mildly spiced vegetable korma and coconut chutney",1, 251, "../../Assets/Images/Dish/poori.png", 14,7},
                                new ArrayList{"Idly",50, "Idli is a diet food and very healthy as it is steamed. Made from urad dal and rice. Provided with a chutney",1, 303, "../../Assets/Images/Dish/idli.png", 14,7},
                                new ArrayList{"Dosa",79, "Irresistibly crispy dosa cooked to perfection served with sambhar and chutney.",1, 363, "../../Assets/Images/Dish/dosa.png", 14,7},
                                new ArrayList{"Pongal",75, "A savory dish made of cooked rice and green gram blended with clarified butter,mildly tempered with spices, garnished with cashews and servedhot",1, 202, "../../Assets/Images/Dish/pongal.png", 14,7},
                                new ArrayList{"Mini Tiffin",140, "Idli (1 pc), mini masala dosa, pongal, sweet, mini vada, 2 types of chutney, sambar.",1, 344, "../../Assets/Images/Dish/miniTiffin.png", 14,7},
                                new ArrayList{"Salty Potato Chips",65, "Our homestyle version of the classic potato chips. Guaranteed they wont last long!",1, 121, "../../Assets/Images/Dish/potatoChips.png", 15,7},
                                new ArrayList{"Salty Banana Chips",70, "Our homestyle version of the classic banana chips. Guaranteed they wont last long!",1, 212, "../../Assets/Images/Dish/bananaChips.png", 15,7}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Margherita Pizza",109, "Classic delight with 100% real mozzarella cheese. Available in Cheese Burst, Wheat Thin Crust and Pan Crust options.",1,13,"../../Assets/Images/Dish/pizza.png",22,8},
                                new ArrayList{"Indi Tandoori Paneer Pizza",299, "It is hot. It is spicy. It is oh-so-Indian. Tandoori paneer with capsicum, red paprika & mint mayo. Available in Cheese Burst, Wheat Thin Crust and Pan Crust options.",1,425,"../../Assets/Images/Dish/pizza.png",22,8},
                                new ArrayList{"Farmhouse Pizza",259, "Delightful combination of onion, capsicum, tomato & grilled mushroom. Available in Cheese Burst, Wheat Thin Crust and Pan Crust options.", 1,451,"../../Assets/Images/Dish/pizza.png",22,8},
                                new ArrayList{"The Cheese Dominator",349, "Loaded with 1 Pound of Mozzarella and gooey Liquid Cheese on a Classic Large Pizza topped with a generous helping of herb sprinkle", 1,101,"../../Assets/Images/Dish/pizza.png",22,8},
                                new ArrayList{"Pepper Barbeque Chicken Pizza",249, "Pepper barbecue chicken for that extra zing. Available in Cheese Burst, Wheat Thin Crust and Pan Crust options.", 0,405,"../../Assets/Images/Dish/pizza.png",23,8},
                                new ArrayList{"Chicken Dominator Pizza",359, "Loaded with double pepper barbecue chicken, peri-peri chicken, chicken tikka & grilled chicken rashers. Available in Cheese Burst, Wheat Thin Crust and Pan Crust options.", 0,425,"../../Assets/Images/Dish/pizza.png",23,8},
                                new ArrayList{"Chicken Maximus",399, "Loaded to the Max with Chicken Tikka, Chicken Keema, Chicken Sausage and a double dose of grilled Chicken Rashers.", 0,231,"../../Assets/Images/Dish/pizza.png",23,8},
                                new ArrayList{"Chicken Fiesta Pizza",309, "Grilled chicken rashers, peri-peri chicken, onion & capsicum, a complete fiesta", 0,446,"../../Assets/Images/Dish/pizza.png",23,8},
                                new ArrayList{"Pepsi",57.14, "Sparkling and Refreshing Beverage", 1,383,"../../Assets/Images/Dish/pepsi.png",7,8},
                                new ArrayList{"Mirinda",57.14, "Sparkling and Refreshing Beverage", 1,179,"../../Assets/Images/Dish/mirinda.png",7,8},
                                new ArrayList{"Choco Lava Cake",109, "Chocolate lovers delight! Indulgent, gooey molten lava inside chocolate cake", 1,496,"../../Assets/Images/Dish/chocoSundae.png",4,8},
                                new ArrayList{"Butterscotch Mousse Cake", 103.81, "Sweet temptation! Butterscotch flavored mousse", 1, 42, "../../Assets/Images/Dish/mousse.png", 4, 8 }
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Dahi Papdi",125, "pappadi topped with aloo masala in curd,mint and sweet chutney drizzled with masala",1, 111, "../../Assets/Images/Dish/dahiPapdi.png", 16,9},
                                new ArrayList{"Pani Puri",180, "Chatpata masala pani puri served with boondi & aloo, ragda & moong.",1, 222, "../../Assets/Images/Dish/paniPuri.png", 16,9},
                                new ArrayList{"Bhel Puri",90, "A delectable combination of papadis, puffed rice, sev, onions, potatoes, tomatoes, raw mango, sprouts, coriander and blend of chutneys",1, 333, "../../Assets/Images/Dish/bhelPuri.png", 16,9},
                                new ArrayList{"Masala Puri",95, "Chatpata masala pani puri served with boondi & aloo, ragda & moong.",1, 211, "../../Assets/Images/Dish/masalaPuri.png", 16,9},
                                new ArrayList{"Salty Potato Chips",65, "Our homestyle version of the classic potato chips. Guaranteed they wont last long!",1, 122, "../../Assets/Images/Dish/potatoChips.png", 16,9},
                                new ArrayList{"Salty Banana Chips",70, "Our homestyle version of the classic banana chips. Guaranteed they wont last long!",1, 90, "../../Assets/Images/Dish/bananaChips.png", 16,9},
                                new ArrayList{"Halwa",68, "A simple Indian sweet made with milk, ghee and sugar.",1, 400, "../../Assets/Images/Dish/halwa.png", 17,9},
                                new ArrayList{"Dry Jamun",90, "A popular dessert consisting of Khova made from milk solids soaked with an aromatic syrup",1, 300, "../../Assets/Images/Dish/gulabJamun.png", 17,9},
                                new ArrayList{"Paneer Jamun",110, "Cute pink gulab jamun with the addition of paneer with a pleasing rose flavor",1, 321, "../../Assets/Images/Dish/paneerJamun.png", 17,9},
                                new ArrayList{"Laddu",130, "Succulent golden yellow laddus made out of numerous small globules of gram flour batter deep fried in clarified butter soaked in sugar syrup with cashew nuts and raisins",1, 141, "../../Assets/Images/Dish/laddu.png", 17,9},
                                new ArrayList{"Jangiri",160, "A traditional sweet fried out of urad dal dough and soaked with aromatic sugar syrup",1, 270, "../../Assets/Images/Dish/jangiri.png", 17,9}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Chicken Biriyani",299, "Our age-old classic Chicken Biryani with hand-ground spices from Hyderabad - Served With Boiled egg, Raita & Salna.",0, 391, "../../Assets/Images/Dish/chickenBiriyani.png", 1,10},
                                new ArrayList{"Mutton Biriyani",399, "Our age-old classic Mutton Biryani with hand-ground spices from Hyderabad - Served With Boiled egg, Raita & Salna.",0, 237, "../../Assets/Images/Dish/muttonBiriyani.png", 1,10},
                                new ArrayList{"Egg Biriyani",250, "Our age-old classic Egg Biryani with hand-ground spices from Hyderabad - Served With Boiled egg, Raita & Salna.",0, 272, "../../Assets/Images/Dish/eggBiriyani.png", 1,10},
                                new ArrayList{"Chicken 65",299, "Chicken 65 is a popular deep fried spicy chicken appetizer served with fried curry leaves roundels onion and lemon wages",0, 64, "../../Assets/Images/Dish/chicken65.png", 3,10},
                                new ArrayList{"Pepper Chicken",340, "Fried chicken in spicy brown gravy laced with lime for tanginess, coated with flavours from crushed pepper corns and spices.",0, 212, "../../Assets/Images/Dish/pepperChicken.png", 3,10},
                                new ArrayList{"Chettinad Chicken Gravy",340, "South India's most flavorful dish made with chicken, fresh ground spices & herbs.",0, 323, "../../Assets/Images/Dish/chettinadChicken.png", 3,10},
                                new ArrayList{"Chicken Wings",230, "Chicken wings mixed with a blend of flavors to offer you a mind-blowing appetizer....",0, 498, "../../Assets/Images/Dish/friedWings.png", 18,10},
                                new ArrayList{"Juicy \"N\" Crunchy",220, "A crispy appetizer made of boneless chicken chunks, coated with batter and fried for a perfect golden colour.",0, 34, "../../Assets/Images/Dish/friedChicken.png", 18,10},
                                new ArrayList{"Spicy \"N\" Crunchy",230, "A spicy appetizer made of boneless chicken chunks, coated with batter and fried for a perfect golden colour.",0, 379, "../../Assets/Images/Dish/friedChicken.png", 18,10},
                                new ArrayList{"Chicken Lollypop",295, "Home-spiced marinated chicken lollipop dipped in a flavorful batter and deep fried to perfection.",0, 57, "../../Assets/Images/Dish/friedLollypop.png", 18,10},
                                new ArrayList{"Bread Halwa",55, "An Indian sweet made with bread cooked in ghee",1, 195, "../../Assets/Images/Dish/breadHalwa.png", 4,10}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Pongal",95, "A savory dish made of cooked rice and green gram blended with clarified butter,mildly tempered with spices, garnished with cashews and servedhot",1, 195, "../../Assets/Images/Dish/pongal.png", 14,11},
                                new ArrayList{"Mini Idly",100, "Idli is a diet food and very healthy as it is steamed. Made from urad dal and rice. Provided with a chutney",1, 52, "../../Assets/Images/Dish/miniIdly.png", 14,11},
                                new ArrayList{"Idiyappam",110, "An authentic and a traditional rice noodle made by steaming the rice flour pressed through Idiyappam maker served with authentic vadacurry",1, 15, "../../Assets/Images/Dish/idiyappam.png", 14,11},
                                new ArrayList{"Poori",100, "A delicious Indian breakfast dish comprising of 2 big circular fluffy deep fried wheat flour Pooris served with a mildly spiced vegetable korma and coconut chutney",1, 138, "../../Assets/Images/Dish/poori.png", 14,11},
                                new ArrayList{"Masala Dosai",129, "Tawa- toasted, crispy and delicious dosa, folded in with masala.",1, 444, "../../Assets/Images/Dish/dosa.png", 14,11},
                                new ArrayList{"Mini Tiffin",160, "Idli (1 pc), mini masala dosa, pongal, sweet, mini vada, 2 types of chutney, sambar.",1, 219, "../../Assets/Images/Dish/miniTiffin.png", 14,11},
                                new ArrayList{"Bajji",120, "Peeled raw bananas slices dipped in gram flour and deep fried in oil.. Perfect matches for evening snacks time.",1, 341, "../../Assets/Images/Dish/bajji.png", 16,11},
                                new ArrayList{"Bonda",130, "Deep fried,and semi-crunchy lentil balls, served with coconut chutney and flavorful sambar",1, 418, "../../Assets/Images/Dish/bonda.png", 16,11},
                                new ArrayList{"Assorted Sweets",178, "Consisting of various handmade ghee & Milk sweets.",1, 184, "../../Assets/Images/Dish/assortedSweets.png", 17,11},
                                new ArrayList{"Rava Kesari",155, "A succulent sweet South Indian dessert made of fine semolina cooked in clarified butter with diced cashew nuts, raisins and served hot",1, 471, "../../Assets/Images/Dish/kesari.png", 17,11},
                                new ArrayList{"Lassi",70, "A creamy, frothy yogurt-based drink of Punjab, sweetened with sugar.",1, 328, "../../Assets/Images/Dish/lassi.png", 7,11},
                                new ArrayList{"Apple Juice",75, "A healthy fruit juice made by the maceration and pressing of an apple.",1, 445, "../../Assets/Images/Dish/appleJuice.png", 7,11},
                                new ArrayList{"Butter Milk",60, "Good old classic indian go to drink to keep the body cool and hydrated. Milk, curd and lemon churned in a pot to relish our thirs",1, 54, "../../Assets/Images/Dish/butterMilk.png", 7,11}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Chicken Zinger",199.05, "Chicken zinger with a delicious tandoori sauce",0, 66, "../../Assets/Images/Dish/burger.png", 8,12},
                                new ArrayList{"Veg Zinger",179.05,"Signature veg burger with crispy patties, veggies & a tangy sauce",1,465, "../../Assets/Images/Dish/burger.png", 8,12},
                                new ArrayList{"Hot and Crispy Chicken - 2Pc",229.52, "Signature Hot & crispy chicken",0, 39, "../../Assets/Images/Dish/chick2Pc.png", 13,12},
                                new ArrayList{"Hot and Crispy Chicken - 8Pc",748.57, "Signature Hot & crispy chicken",0, 142, "../../Assets/Images/Dish/chick8Pc.png", 13,12},
                                new ArrayList{"Medium French Fries",99.05, "Jazz Up Your Meal With Crispy Fries!",1, 234, "../../Assets/Images/Dish/fries.png", 12,12},
                                new ArrayList{"Large French Fries",119.05, "Jazz Up Your Meal With Crispy Fries!",1, 426, "../../Assets/Images/Dish/fries.png", 12,12},
                                new ArrayList{"Tandoori Masala Dip",28.57, "Special tandoori flavored dip, to add a twist to your meal!",1, 97, "../../Assets/Images/Dish/masalaDip.png", 19,12},
                                new ArrayList{"Choco Lava Cake",108.57, "New Soft Chocolate cake with a gooey center- perfect chocolaty end to every meal",1, 312, "../../Assets/Images/Dish/chocoVolcano.png", 4,12},
                                new ArrayList{"Choco Mud Pie",128.57, "Chocolate lovers unite! Say hello to our delicous, new, creamy chocolate & cake dessert- a must try!",1, 306, "../../Assets/Images/Dish/mudPie.png", 4,12},
                                new ArrayList{"Pepsi",57.14,"A Soft Drink.",1,52, "../../Assets/Images/Dish/pepsi.png", 7,12},
                                new ArrayList{"7Up",57.14, "A Soft Drink.",1, 154, "../../Assets/Images/Dish/7up.png", 7,12},
                                new ArrayList{"Mirinda",57.14, "A Soft Drink.",1, 497, "../../Assets/Images/Dish/mirinda.png", 7,12}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Pongal",75, "A savory dish made of cooked rice and green gram blended with clarified butter,mildly tempered with spices, garnished with cashews and servedhot",1, 195, "../../Assets/Images/Dish/pongal.png", 14,13},
                                new ArrayList{"Mini Idly",110, "Idli is a diet food and very healthy as it is steamed. Made from urad dal and rice. Provided with a chutney",1, 52, "../../Assets/Images/Dish/miniIdly.png", 14,13},
                                new ArrayList{"Idiyappam",90, "An authentic and a traditional rice noodle made by steaming the rice flour pressed through Idiyappam maker served with authentic vadacurry",1, 15, "../../Assets/Images/Dish/idiyappam.png", 14,13},
                                new ArrayList{"Poori",94, "A delicious Indian breakfast dish comprising of 2 big circular fluffy deep fried wheat flour Pooris served with a mildly spiced vegetable korma and coconut chutney",1, 138, "../../Assets/Images/Dish/poori.png", 14,13},
                                new ArrayList{"Masala Dosai",106, "Tawa- toasted, crispy and delicious dosa, folded in with masala.",1, 444, "../../Assets/Images/Dish/dosa.png", 14,13},
                                new ArrayList{"Mini Tiffin",150, "Idli (1 pc), mini masala dosa, pongal, sweet, mini vada, 2 types of chutney, sambar.",1, 219, "../../Assets/Images/Dish/miniTiffin.png", 14,13},
                                new ArrayList{"Sambar Rice",75, "Sambar rice also known as sambar sadam is one of the everyday foods from the Tamil cuisine.",1, 36, "../../Assets/Images/Dish/sambarRice.png", 20,13},
                                new ArrayList{"Karuveppillai Rice",75, "Karuveppilai rice is a tasty, rich, and delectable delicacy. When making karuveppilai rice, curry leaves are thoroughly cleaned, cooked till the ideal consistency, and then served hot.",1, 493, "../../Assets/Images/Dish/KaruveppilaiRice.png", 20,13},
                                new ArrayList{"Biriyani",180, "Rice and veggies cooked in a fragrant and flavorful masala seasoned with indian whole spices. Served with salan and raita",1, 286, "../../Assets/Images/Dish/vegBiriyani.png", 20,13},
                                new ArrayList{"Pulao",206, "A delicious one pot Indian dish made with rice, vegetables, spices and herbs.",1, 178, "../../Assets/Images/Dish/pulao.png", 20,13},
                                new ArrayList{"Fried Rice",206, "An assortment of veggies and white rice tossed in a wok and spiced to perfection",1, 474, "../../Assets/Images/Dish/friedRice.png", 20,13},
                                new ArrayList{"Bajji",120, "Peeled raw bananas slices dipped in gram flour and deep fried in oil.. Perfect matches for evening snacks time.",1, 341, "../../Assets/Images/Dish/bajji.png", 16,13},
                                new ArrayList{"Bonda",130, "Deep fried,and semi-crunchy lentil balls, served with coconut chutney and flavorful sambar",1, 418, "../../Assets/Images/Dish/bonda.png", 16,13},
                                new ArrayList{"Assorted Sweets",178, "Consisting of various handmade ghee & Milk sweets.",1, 184, "../../Assets/Images/Dish/assortedSweets.png", 17,13},
                                new ArrayList{"Rava Kesari",155, "A succulent sweet South Indian dessert made of fine semolina cooked in clarified butter with diced cashew nuts, raisins and served hot",1, 471, "../../Assets/Images/Dish/kesari.png", 17,13},
                                new ArrayList{"Lassi",70, "A creamy, frothy yogurt-based drink of Punjab, sweetened with sugar.",1, 328, "../../Assets/Images/Dish/lassi.png", 7,13},
                                new ArrayList{"Apple Juice",75, "A healthy fruit juice made by the maceration and pressing of an apple.",1, 445, "../../Assets/Images/Dish/appleJuice.png", 7,13},
                                new ArrayList{"Butter Milk",60, "Good old classic indian go to drink to keep the body cool and hydrated. Milk, curd and lemon churned in a pot to relish our thirs",1, 54, "../../Assets/Images/Dish/butterMilk.png", 7,13}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Chicken Big Mac",235, "An Iconic burger with Chicken Double patty, crispy shredded lettuce, internationally sourced iconic sauce and Gherkins.",0, 198, "../../Assets/Images/Dish/burger.png", 8,14},
                                new ArrayList{"McAloo Tikki Burger",58, "The World's favourite Indian burger! Crunchy potato and peas patty with delicious Tom Mayo & crunchy onions; now with Whole Wheat Bun",1, 350, "../../Assets/Images/Dish/burger.png", 8,14},
                                new ArrayList{"McChicken Burger",131, "Tender and juicy chicken patty cooked to perfection, with creamy mayonnaise and crunchy lettuce adding flavour to each bite.",0, 242, "../../Assets/Images/Dish/burger.png", 8,14},
                                new ArrayList{"McVeggie Burger",145, "An everyday classic burger with a delectable patty filled with potatoes, carrots and tasty Indian spices; topped with crunchy lettuce and mayonaise.",1, 0, "../../Assets/Images/Dish/burger.png", 8,14},
                                new ArrayList{"Spicy Paneer Wrap",210, "Rich & filling paneer patty coated in spicy crispy batter, topped with tom mayo sauce wrapped with lettuce, onions and cheese",1, 0, "../../Assets/Images/Dish/wrap.png", 9,14},
                                new ArrayList{"Spicy Chicken Wrap",220, "Tender and juicy chicken patty coated in spicy, crispy batter, topped with a creamy sauce, wrapped with lettuce, onions, seasoning and cheese. A BIG indulgence.",0, 168, "../../Assets/Images/Dish/wrap.png", 9,14},
                                new ArrayList{"Coke",90, "Enjoy a delivery friendly experience with the new, reusable bottle for your favorite beverage. A perfect accompaniment to your burger, fries and everything nice.",1, 241, "../../Assets/Images/Dish/cocaCola.png", 7,14},
                                new ArrayList{"Fanta",90, "Enjoy a delivery friendly experience with the new, reusable bottle for your favorite beverage. A perfect accompaniment to your burger, fries and everything nice.",1, 141, "../../Assets/Images/Dish/fanta.png", 7,14},
                                new ArrayList{"Sprite",90, "Enjoy a delivery friendly experience with the new, reusable bottle for your favorite beverage. A perfect accompaniment to your burger, fries and everything nice.",1, 18, "../../Assets/Images/Dish/sprite.png", 7,14},
                                new ArrayList{"French Fries",120, "World Famous Fries, crispy golden, fried to perfection and lightly salted. Add to your order and enjoy it as is or with your favourite dips and seasoning.",1, 298, "../../Assets/Images/Dish/fries.png", 7,14},
                                new ArrayList{"Cheesy Fries",138, "The worlds best French Fries now served with delicious cheesy and spicy sauces",1, 494, "../../Assets/Images/Dish/fries.png", 10,14},
                                new ArrayList{"Cheesy Veg Nuggets",48.58, "Try the new delicious 2 piece pack of Cheesy Veg Nuggets, oozing with cheese seasoned to perfection. Best enjoyed with Chilli Sauce or Piri Piri seasoning",1, 468, "../../Assets/Images/Dish/mcNuggets.png", 10,14},
                                new ArrayList{"Big Pizza McPuff",51, "Crispy brown crust with a generous helping of rich tomato sauce, mixed with carrots, bell peppers, beans, onions and mozzarella.",1, 227, "../../Assets/Images/Dish/mcPuff.png", 10,14},
                                new ArrayList{"Chicken McNuggets",198.99, "9 pieces of tender, juicy chicken nuggets. Pair it with your favorite dipping sauces!",0, 67, "../../Assets/Images/Dish/mcNuggets.png", 10,14}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Chicken Fried Momo",225, "Deep fried Momo Filled with soft juicy Chicken, Onion, Coriander and serve with the flavour of indian spicy sauce",0, 468, "../../Assets/Images/Dish/momo.png", 6,15},
                                new ArrayList{"Veg Schezwan Momo",215, "Spicy momos Stuffed with celery ,chopped onion followed by Vegtables",1, 373, "../../Assets/Images/Dish/momo.png", 6,15},
                                new ArrayList{"Veg Fried Momo",215, "Deep fried Momo Filled with soft juicy choped veg , Onion, Coriander and serve with the flavour of indian spicy sauce",1, 38, "../../Assets/Images/Dish/momo.png", 6,15},
                                new ArrayList{"Schezwan Chicken Momo",225, "Spicy momos Stuffed with celery ,chopped onion followed by chicken, pepper & saute well",0, 70, "../../Assets/Images/Dish/momo.png", 6,15},
                                new ArrayList{"Thai Chilli Mushroom",235, "Deep fried mushroom sauteed Thai Sessioning with Red Pepper and onion",1, 425, "../../Assets/Images/Dish/chilliMushroom.png", 3,15},
                                new ArrayList{"Crispy Chilli Potato",235, "Potatoes deep fried with green chillies",1, 133, "../../Assets/Images/Dish/chilliPotato.png", 3,15},
                                new ArrayList{"Chilli Chicken",245, "Chicken in spicy soya sauce sauteed with green chillies , onion & capsicum",0, 407, "../../Assets/Images/Dish/chilliChicken.png", 3,15},
                                new ArrayList{"Dragon Chicken",245, "Tossed in sweet chilli sauce and cashew nuts",0, 461, "../../Assets/Images/Dish/dragonChicken.png", 3,15},
                                new ArrayList{"Veg Chilli Garlic Noodles",245, "Tossed with shredded vegetables,Chilly And Burnt garlic -500gm with stir-fry paneer sauce.-200ml",1, 493, "../../Assets/Images/Dish/chilliNoodles.png", 5,15},
                                new ArrayList{"Veg Garlic Fried Rice",245, "Tossed with burnt garlic and chopped vegetables. -500 gm and serve with chilly Cauliflower gravy.-200 ml",1, 178, "../../Assets/Images/Dish/friedRice.png", 5,15},
                                new ArrayList{"Chicken Schezwan Fried Rice",189, "Spicy Schezwan Fried Rice The Best Homemade Sweet, Sour And Hot Sauce.",0, 473, "../../Assets/Images/Dish/friedRice.png", 5,15},
                                new ArrayList{"Chicken Garlic Fried Rice",250, "Tossed with burnt garlic and chopped vegetables and chicken .-500gm with green chillies,red pepper chicken sauce.-200ml",0, 350, "../../Assets/Images/Dish/friedRice.png", 5,15}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Margherita",169, "Pizza topped with our herb-infused signature pan sauce and 100% mozzarella cheese. A classic treat for all cheese lovers out there!",1, 377, "../../Assets/Images/Dish/pizza.png", 22,16},
                                new ArrayList{"Cheese and Corn",219, "A combination of juicy Sweet Corn & 100% mozzarella cheese with flavourful signature pan sauce.",1, 91, "../../Assets/Images/Dish/pizza.png", 22,16},
                                new ArrayList{"Tandoori paneer",319, "It's our signature. Spiced Paneer, Crunchy Onions & Green Capsicum, spicy Red Paprika with delicious Tandoori Sauce and 100% mozzarella cheese!",1, 164, "../../Assets/Images/Dish/pizza.png", 22,16},
                                new ArrayList{"Farmers Pick",319, "Flavourful Herbed Onion and Green, crunchy Red Capsicum, yummy Mushrooms & Baby Corn with flavourful pan sauce and 100% mozzarella cheese.",1, 60, "../../Assets/Images/Dish/pizza.png", 22,16},
                                new ArrayList{"Chicken Sausage",259, "Pizza topped with Chicken n Cheese Sausages & crunchy Onions, flavourful pan sauce and 100% mozzarella cheese.",0, 370, "../../Assets/Images/Dish/pizza.png", 23,16},
                                new ArrayList{"Spiced Chicken Meatballs",319, "Juicy Schezwan Chicken Meatball, crunchy Onion & 100% mozzarella cheese with flavourful pan sauce.",0, 222, "../../Assets/Images/Dish/pizza.png", 23,16},
                                new ArrayList{"Pepperoni",379, "Yummy Chicken Pepperoni & 100% mozzarella cheese with signature pan sauce. It’s a classic!",0, 434, "../../Assets/Images/Dish/pizza.png", 23,16},
                                new ArrayList{"Chicken Supreme",409, "Loaded with delicious Chicken Tikka, flavourful Herbed Chicken, spicy Schezwan Chicken Meatball with flavourful pan sauce and 100% mozzarella cheese.",0, 320, "../../Assets/Images/Dish/pizza.png", 23,16},
                                new ArrayList{"Cheese Garlic Bread",159, "Nothing beats our garlic bread! Period!",1, 5, "../../Assets/Images/Dish/garlicBread.png", 10,16},
                                new ArrayList{"Mexican Garlic Bread",159, "Freshly baked San Francisco Style breadsticks topped with chopped onion, green capsicum, red capsicum, jalapeno, black olives & drizzled with olive oil",1, 399, "../../Assets/Images/Dish/garlicBread.png", 10,16},
                                new ArrayList{"Pepsi",57,"A Soft Drink.",1,348, "../../Assets/Images/Dish/pepsi.png", 7,16},
                                new ArrayList{"Mirinda",57,"A Soft Drink.",1,423, "../../Assets/Images/Dish/mirinda.png", 7,16},
                                new ArrayList{"Choco Sundae",48.33, "Choco Sundae Cup (100 ml)",1, 485, "../../Assets/Images/Dish/chocoSundae.png", 4,16},
                                new ArrayList{"Choco Volcano",129, "Warm choco cake with gooey center",1, 392, "../../Assets/Images/Dish/chocoVolcano.png", 4,16}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{ "Chicken Slice Egg & Cheese Sandwich",199, "Start your day the fresh way with high protein fluffy eggs, chicken slices, and mouth-watering cheese on a freshly baked bread.",0,284, "../../Assets/Images/Dish/sandwich.png", 24,17},
                                new ArrayList{ "Aloo Patty Sandwich",209, "Aloo patty seasoned with special herbs and spices, paired with your choice of your favourite nutritious veggies, on a freshly baked bread.", 1,116, "../../Assets/Images/Dish/sandwich.png", 24,17},
                                new ArrayList{ "Chicken Meatball Sandwich",249, "Authentic indian meatballs perfectly seasoned and spiced served along with wholesome veggies and packed in your choice of freshly baked bread.", 0,472, "../../Assets/Images/Dish/sandwich.png", 24,17},
                                new ArrayList{ "Paneer Tikka Sandwich",259, "Your favourite cheese cubes marinated with tandoori sauce, spices and condiments, cooked in an oven to give a smoky taste. Served with a choice of nutritious veggies and a freshly baked bread.", 1,18, "../../Assets/Images/Dish/sandwich.png", 24,17},
                                new ArrayList{ "Mexican Bean Patty Salad", 279, "Want to try something with a kick, then go for the mexican patty. A tasty patty made with red and black kidney beans, combined with your favourite veggies and dressing to give you a masterpiece.", 1,78, "../../Assets/Images/Dish/salad.png", 25,17},
                                new ArrayList{ "Paneer Tikka Salad", 299, "Want a familiar taste taken up a notch try the paneer tikka salad. Perfectly cooked paneer tikka combined with fresh veggies and a dressing of your choice.", 1,372, "../../Assets/Images/Dish/salad.png", 25,17},
                                new ArrayList{ "Chicken Meatball Salad", 319, "The chicken kofta salad is the ultimate cravings crusher. Authentic indian meatballs sitting on a bed of your favourite veggies along with your favourite dressing.", 0,299, "../../Assets/Images/Dish/salad.png", 25,17},
                                new ArrayList{ "Mexican Patty Signature Wrap", 279, "Enjoy this serving filled with double portion of mexican patty you love, on a tortilla of your choice. Top it with your favourite veggies and sauces.", 1,467, "../../Assets/Images/Dish/wrap.png",9,17},
                                new ArrayList{ "Smoked Chicken Strips Signature Wrap", 319, "A classic that packs the goodness of double the filling. Double portion of strips of smoked chicken, along with nutritious veggies and delicious sauces, served inside a tortilla of your choice.", 0,334, "../../Assets/Images/Dish/wrap.png", 9,17},
                                new ArrayList{ "Pepsi",57.14, "Sparkling and Refreshing Beverage", 1,12, "../../Assets/Images/Dish/pepsi.png", 7,17},
                                new ArrayList{ "Mirinda",57.14, "Sparkling and Refreshing Beverage", 1,14, "../../Assets/Images/Dish/mirinda.png", 7,17},
                                new ArrayList{ "Oatmeal Raisin Cookie", 75, "Like something extra and sweet add some sweetness with your favourite oatmeal raisin cookies.", 0,139, "../../Assets/Images/Dish/cookie.png", 4,17},
                                new ArrayList{ "Dark Chunk Chocolate Cookie", 75, "Craving for chocolate, now enjoy your favourite dark chunk chocolate cookie.", 0,432, "../../Assets/Images/Dish/cookie.png", 4,17}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Veg Clear Soup", 145, "A simple soup of assorted vegetables cooked in a lightly flavoured stock.",1, 200, "../../Assets/Images/Dish/vegClearSoup.png", 2, 18},
                                new ArrayList{"Chicken Clear Soup", 155, "A delectable soup prepared by simmering chicken slivers in a delightful hot and sour stock",0, 300, "../../Assets/Images/Dish/vegClearSoup.png", 2, 18},
                                new ArrayList{"Paneer 65",265, "Deep-fried Paneer, marinated in a variety of spices and cooked to perfection.",1, 220, "../../Assets/Images/Dish/paneer65.png", 3,18},
                                new ArrayList{"Chicken 65",310, "Chicken 65 is a popular deep fried spicy chicken appetizer served with fried curry leaves roundels onion and lemon wages",0, 64, "../../Assets/Images/Dish/chicken65.png", 3,18},
                                new ArrayList{"Veg Fried Rice",206, "An assortment of veggies and white rice tossed in a wok and spiced to perfection",1, 474, "../../Assets/Images/Dish/friedRice.png", 5,18},
                                new ArrayList{"Chicken Biriyani",345, "Richly flavored aromatic rice layered with succulent chicken in a delicate blend of whole spices; served along with raita and curry.",0, 205, "../../Assets/Images/Dish/chickenBiriyani.png", 5,18},
                                new ArrayList{"Mutton Biriyani",345, "Richly flavored aromatic rice layered with succulent mutton in a delicate blend of whole spices; served along with raita and curry.",0, 408, "../../Assets/Images/Dish/muttonBiriyani.png", 5,18},
                                new ArrayList{"Egg Biriyani",255, "Healthy yet wholesome boiled eggs covered in flavor-packed masala and slow cooked rice.",0, 90, "../../Assets/Images/Dish/eggBiriyani.png", 5,18},
                                new ArrayList{"Chicken Fried Rice",275, "A slightly spicy dish made by tossing juicy chicken and rice in a flavorful sauce.",0, 112, "../../Assets/Images/Dish/friedRice.png", 5,18},
                                new ArrayList{"Bread Halwa",55, "An Indian sweet made with bread cooked in ghee",1, 195, "../../Assets/Images/Dish/breadHalwa.png", 4,18}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Vanilla Cake",350, "Moist and delicious cake made with original vanilla crush and fresh cream.",0,118,"../../Assets/Images/Dish/cake.png",26,19},
                                new ArrayList{"Black Forest Cake", 350, "Layers of chocolate sponge, whipped cream, cherries, chocolate shavings.", 0, 251, "../../Assets/Images/Dish/cake.png", 26, 19 },
                                new ArrayList{"White Forest Cake", 350, "Layered between cherry filling and a white chocolate whipped cream frosting topped with white chocolate shavings.", 0, 362, "../../Assets/Images/Dish/cake.png", 26, 19 },
                                new ArrayList{"Veg Puff", 25, "Mixed vegetable in sheeted dough", 1, 42, "../../Assets/Images/Dish/puff.png", 16, 19 },
                                new ArrayList{"Egg Puff", 33, "Mixed vegetable and Chicken in sheeted dough", 0, 31, "../../Assets/Images/Dish/puff.png", 16, 19 },
                                new ArrayList{"Chicken Puff", 37, "Mixed vegetable and Egg in sheeted dough", 0, 184, "../../Assets/Images/Dish/puff.png", 16, 19 },
                                new ArrayList{"White Fudge Donut", 51, "Milk chocolate fudge doughnuts with a soft chocolate glaze, white chocolate and chocolate curls.", 1, 91, "../../Assets/Images/Dish/donut.png", 4, 19 },
                                new ArrayList{"Chocolate Fudge Donut", 51, "Chocolate fudge doughnuts with a soft chocolate glaze, white chocolate and chocolate curls.", 1, 434, "../../Assets/Images/Dish/donut.png", 4, 19 },
                                new ArrayList{"Dark Chocolate Brownie", 75, "Home made brownie comes with various topping to choose from.", 0, 9, "../../Assets/Images/Dish/brownie.png", 4, 19 },
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Veg Clear Soup", 140, "A simple soup of assorted vegetables cooked in a lightly flavoured stock.", 1, 200, "../../Assets/Images/Dish/vegClearSoup.png", 2, 20 },
                                new ArrayList{"Chicken Clear Soup", 160, "A delectable soup prepared by simmering chicken slivers in a delightful hot and sour stock", 0, 300, "../../Assets/Images/Dish/vegClearSoup.png", 2, 20 },
                                new ArrayList{"Paneer 65", 240, "Deep-fried Paneer, marinated in a variety of spices and cooked to perfection.", 1, 220, "../../Assets/Images/Dish/paneer65.png", 3, 20 },
                                new ArrayList{"Chicken 65", 270, "Chicken 65 is a popular deep fried spicy chicken appetizer served with fried curry leaves roundels onion and lemon wages", 0, 64, "../../Assets/Images/Dish/chicken65.png", 3, 20 },
                                new ArrayList{"Chicken Biriyani", 300, "Richly flavored aromatic rice layered with succulent chicken in a delicate blend of whole spices; served along with raita and curry.", 0, 205, "../../Assets/Images/Dish/chickenBiriyani.png", 5, 20 },
                                new ArrayList{"Mutton Biriyani", 240, "Richly flavored aromatic rice layered with succulent mutton in a delicate blend of whole spices; served along with raita and curry.", 0, 408, "../../Assets/Images/Dish/muttonBiriyani.png", 5, 20 },
                                new ArrayList{"Egg Biriyani", 200, "Healthy yet wholesome boiled eggs covered in flavor-packed masala and slow cooked rice.", 0, 90, "../../Assets/Images/Dish/eggBiriyani.png", 5, 20 },
                                new ArrayList{"Chicken Fried Rice", 275, "A slightly spicy dish made by tossing juicy chicken and rice in a flavorful sauce.", 0, 112, "../../Assets/Images/Dish/friedRice.png", 5, 20 },
                                new ArrayList{"Chicken Noodles", 250, "An aromatic and mouthwatering dish prepared from noodles and strips of juicy chicken.", 0, 46, "../../Assets/Images/Dish/friedRice.png", 5, 20 },
                                new ArrayList{"French Fries", 150, "Jazz Up Your Meal With Crispy Fries!", 1, 234, "../../Assets/Images/Dish/fries.png", 21, 20 },
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Sweet Cream Scoop",69, "Rich creamy vanilla dessert thats more vanilla-tasting than any vanilla you have ever tasted in all of vanillas entirety.", 1,455, "../../Assets/Images/Dish/scoop.png", 27,21},
                                new ArrayList{"Chocoate Scoop",104, "Finest chocolate scoop made with rich cocoa. So rich & chocolatey that will make you go Choco Doodle Do", 1,358, "../../Assets/Images/Dish/scoop.png", 27,21},
                                new ArrayList{"Blackcurrant Scoop",104, "Finest Blackcurrant scoop", 1,300,"../../Assets/Images/Dish/scoop.png",27,21},
                                new ArrayList{"Banana Milk Shake",166, "Try this aromatic milkshake loaded with fresh banana with the sweetened rose petals and rich creamy ice cream.", 1,328,"../../Assets/Images/Dish/milkshake.png",28,21},
                                new ArrayList{"Bubblegum Milk Shake",166, "Try this aromatic milkshake loaded with Bubblegum Flavour.", 1,105, "../../Assets/Images/Dish/milkshake.png", 28,21},
                                new ArrayList{"Caramel Milk Shake",166, "Try this rich thickshake with the goodness of salted caramel and and rich creamy ice cream", 1,349, "../../Assets/Images/Dish/milkshake.png", 28,21},
                                new ArrayList{"Butterscotch Pastry",94, "Homemade butterscotch with sponge cake.", 0,233,"../../Assets/Images/Dish/pastries.png",29,21},
                                new ArrayList{"Chocolate Truffle Pastry",94, " Layers of soft chocolate sponge and dense but silky-smooth chocolate ganache make this cake a decadent celebration.", 0,100,"../../Assets/Images/Dish/pastries.png",29,21},
                                new ArrayList{"Oreo Pastry",94, "A Spongy Cake made with famous and imported Oreo cookies.", 0,54,"../../Assets/Images/Dish/pastries.png",29,21},
                                new ArrayList{"Coffee Waffle",129, "Chocolate Waffle Cotted With Coffee Filling", 1,435,"../../Assets/Images/Dish/waffle.png",30,21},
                                new ArrayList{"Belgian Chocolate",135, "Chocolate Waffle Cotted With Milky Chocolate", 1,162,"../../Assets/Images/Dish/waffle.png",30,21},
                                new ArrayList{"Butter Honey Waffle",124, "Orginal Waffle Toped With Butter And Honey", 1,230,"../../Assets/Images/Dish/waffle.png",30,21}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Dilli Ke Chole Lasagne",239, "Classic lasagna filled with chatpate chole and layers of chipotle liquid cheese will make your taste buds do the Patiala tango.",1,494,"../../Assets/Images/Dish/lasagne.png",31,22},
                                new ArrayList{"Chicken Keema Lasagne",299, "Our signature Italian dish fused with desi flavors - delicious lasagna layered with Kheema and oozing with cheese. Desi taste, Italian ways.",0,262,"../../Assets/Images/Dish/lasagne.png",31,22},
                                new ArrayList{"Meaty Meatball Lasagne",299, "Satisfy your meat cravings with this double dose of chicken meatball! Made even more delicious with our marinara sauce and chipotle cheese exuding out of every layer.",0,354,"../../Assets/Images/Dish/lasagne.png",31,22},
                                new ArrayList{"Chatpate Chole Quesadilla", 269, "Mouth-watering quesadillas filled with veggies and cheese layered between whole wheat lachha paratha making it guilt-free.",1,463,"../../Assets/Images/Dish/quesadilla.png",32,22},
                                new ArrayList{"Kheema Paratha Quesadilla",299, "Try our funked up desi version of a quesadilla stuffed with chicken kheema, freshly baked veggies and a spread of hot and tangy chipotle sauce to notch it up. All in an exciting package of these grilled tacos.",0,392,"../../Assets/Images/Dish/quesadilla.png",32,22},
                                new ArrayList{"Smokey Sausage Quesadilla",299, "Bite into the flavour of smoked chicken sausages with a tadkedaar touch of bell peppers and our secret spice mix to get the temperatures rising. All in an exciting package of these grilled tacos.",0,234,"../../Assets/Images/Dish/quesadilla.png",32,22},
                                new ArrayList{"Grilled Chicken Nuggets with Mayo Dip",190, "Grilled minced chicken inspired from Persian flavors. Served with mayo dip.",0,364,"../../Assets/Images/Dish/nuggets.png",3,22},
                                new ArrayList{"Cheese Infused Garlic Bread",135, "Fresh flatbread baked with garlic butter and chefs secret seasoning, infused with creamy el-classico cheese. Served with a cheese dip to make it an extra cheesy affair!",1,388,"../../Assets/Images/Dish/garlicBread.png",3,22},
                                new ArrayList{"Swig - Green Apple",59, "Aerated drink flavored with the mouth-puckering green apple. Sure to refresh you completely!",1,130,"../../Assets/Images/Dish/swig.png",7,22},
                                new ArrayList{"Coca-Cola",57, "Aerated drink flavored with the mouth-puckering cola",1,298,"../../Assets/Images/Dish/cocaCola.png", 7,22}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Butterscotch Milkshake",229, "Who doesn't love butterscotch? Try this which and creamy shake loaded with goodness of butterscotch flavour and rich creamy ice cream",1,359, "../../Assets/Images/Dish/milkshake.png", 33,23},
                                new ArrayList{"Filter Kaapi Milkshake", 229, "Craving for an authentic south Indian coffee? Why not try this thickshake filled with the aroma of roasted coffee beans and rich creamy ice cream", 1, 174, "../../Assets/Images/Dish/milkshake.png", 33,23},
                                new ArrayList{"Snickers Surprise Milkshake", 249, "Who doesn't love butterscotch? Try this which and creamy shake loaded with goodness of butterscotch flavour and rich creamy ice cream", 1, 194, "../../Assets/Images/Dish/milkshake.png", 33,23},
                                new ArrayList{"Bubble Gum Milkshake", 249, "Who doesn't love butterscotch? Try this which and creamy shake loaded with goodness of butterscotch flavour and rich creamy ice cream", 1, 324, "../../Assets/Images/Dish/milkshake.png", 33,23},
                                new ArrayList{"Candy Crush Milkshake", 249, "Who doesn't love butterscotch? Try this which and creamy shake loaded with goodness of butterscotch flavour and rich creamy ice cream", 1, 327, "../../Assets/Images/Dish/milkshake.png", 33,23},
                                new ArrayList{"Strawberry", 99, "The sweet-tart burst of ripe, juicy strawberries stirred into sweet, frozen cream", 1, 367, "../../Assets/Images/Dish/scoop.png", 34,23},
                                new ArrayList{"Alphonso Mango", 99, "Flavoured with the King of Fruits! Mangoes impart a luscious creamy texture and rich flavour to this delicious dessert.", 1, 171, "../../Assets/Images/Dish/scoop.png", 34,23},
                                new ArrayList{"Creamy Vanilla", 169, "Goodness of aromatic vanilla beans in this scoop of rich and creamy Original Vanilla ice cream", 1, 276, "../../Assets/Images/Dish/scoop.png", 34,23},
                                new ArrayList{"Dry Fruit Delight", 139, "Loaded with dry fruits and nuts this ice cream is a delight in every bite.", 1, 421, "../../Assets/Images/Dish/scoop.png", 34,23},
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Bhuna Chicken Rice Bowl", 299, "Tender chicken cooked in fresh bhuna spices, served with flavoured rice and salad is here to make your day!", 0, 208, "../../Assets/Images/Dish/riceBowl.png", 11, 24},
                                new ArrayList{"Chicken Kheema Rice Bowl", 299, "Let the good times roll with this all-time favourite bowl. Minced chicken kheema with Hyderabadi rice and a side of salad. Must try!", 0, 329, "../../Assets/Images/Dish/riceBowl.png", 11, 24},
                                new ArrayList{"Smoked Butter Chicken Rice Bowl",255, "Everyone loves a good butter chicken and this one is made with added excitement to surprise your tastebuds! Served over a bed of Spicy Hyderabadi Rice or Classic Flavourful Rice",0, 255, "../../Assets/Images/Dish/riceBowl.png", 11,24},
                                new ArrayList{"Paneer Signature Rice Bowl",229, "Let your taste buds fall in love with this unique bowl made using out-of-the-world paneer tikka chunks served over a bed of Spicy Hyderabadi Rice or Classic Flavourful Rice",1, 229, "../../Assets/Images/Dish/riceBowl.png", 11,24},
                                new ArrayList{"Roast Vegetables Rice Bowl", 249, "Exotic vegetables cooked in creamy gravy and served with Hyderabadi rice. What else do you want", 1, 458, "../../Assets/Images/Dish/riceBowl.png", 11, 24},
                                new ArrayList{"Masala Paneer Tikka Wrap",175, "Fresh paneer is smoked to perfection & drizzled with minty, spicy mayonnaise & wrapped in soft roti.",1, 111, "../../Assets/Images/Dish/wrap.png", 9,24},
                                new ArrayList{"Cheese Melt Chicken Bhuna Wrap",205, "Craving for something new? We got your back. We mean, we got you a wrap. Chunks of chicken cooked in bhuna masala, topped with sinful cheese into a soft roti.",0, 360, "../../Assets/Images/Dish/wrap.png", 9,24},
                                new ArrayList{"Cheesy Chicken Meatballs",129, "Juicy minced chicken meatballs, served with a cheesy mayonnaise dip - A no-brainer for snack cravings!",0, 129, "../../Assets/Images/Dish/meatballs.png", 10,24},
                                new ArrayList{"Potato Wedges",99, "When you dont know what to eat next we suggest keep it simple and call for the wedges.",1, 99, "../../Assets/Images/Dish/wedges.png", 10,24},
                                new ArrayList{"Cheese Cake",179, " This New York Style Mango Cheesecake, made with premium quality cream cheese and a crust infused with delicious mango flavor, is every dessert lovers delight!",1, 179, "../../Assets/Images/Dish/cheeseCake.png", 4,24},
                                new ArrayList{"Brownie",119, "Gooey & fudgy on the inside and nutty on the outside. Our melt-in-the-mouth Single Nutty Hazelnut Brownie will give you serious dessert goals.",1, 119, "../../Assets/Images/Dish/brownie.png", 4,24},
                                new ArrayList{"Coca - Cola",57, "Coca-Cola Bottle.",1, 155, "../../Assets/Images/Dish/cocaCola.png", 7,24},
                                new ArrayList{"Swig",59, "Aerated drink flavored with the mouth-puckering green apple. Sure to refresh you completely!",1, 55, "../../Assets/Images/Dish/swig.png", 7,24},
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Paneer 65",265, "Deep-fried Paneer, marinated in a variety of spices and cooked to perfection.",1, 220, "../../Assets/Images/Dish/paneer65.png", 3,25},
                                new ArrayList{"Chicken 65",310, "Chicken 65 is a popular deep fried spicy chicken appetizer served with fried curry leaves roundels onion and lemon wages",0, 64, "../../Assets/Images/Dish/chicken65.png", 3,25},
                                new ArrayList{"Pepper Chicken",340, "Fried chicken in spicy brown gravy laced with lime for tanginess, coated with flavours from crushed pepper corns and spices.",0, 212, "../../Assets/Images/Dish/pepperChicken.png", 3,25},
                                new ArrayList{"Chicken Biriyani",345, "Richly flavored aromatic rice layered with succulent chicken in a delicate blend of whole spices; served along with raita and curry.",0, 205, "../../Assets/Images/Dish/chickenBiriyani.png", 1,25},
                                new ArrayList{"Mutton Biriyani",345, "Richly flavored aromatic rice layered with succulent mutton in a delicate blend of whole spices; served along with raita and curry.",0, 408, "../../Assets/Images/Dish/muttonBiriyani.png", 1,25},
                                new ArrayList{"Egg Biriyani",255, "Healthy yet wholesome boiled eggs covered in flavor-packed masala and slow cooked rice.",0, 90, "../../Assets/Images/Dish/eggBiriyani.png", 1,25},
                                new ArrayList{"Pepsi",57,"A Soft Drink.",1,348, "../../Assets/Images/Dish/pepsi.png", 7,25},
                                new ArrayList{"Mirinda",57,"A Soft Drink.",1,423, "../../Assets/Images/Dish/mirinda.png", 7,25},
                                new ArrayList{"Gulab Jamun",95, "Gulab Jamun. Soft. Plump and the way we are used to. There is nothing else in our gulab jamuns than authenticity and love.",1, 80, "../../Assets/Images/Dish/gulabJamun.png", 4,25},
                                new ArrayList{"Bread Halwa",55, "An Indian sweet made with bread cooked in ghee",1, 195, "../../Assets/Images/Dish/breadHalwa.png", 4,25}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Veg Clear Soup", 140, "A simple soup of assorted vegetables cooked in a lightly flavoured stock.", 1, 200, "../../Assets/Images/Dish/vegClearSoup.png", 2, 26 },
                                new ArrayList{"Chicken Clear Soup", 160, "A delectable soup prepared by simmering chicken slivers in a delightful hot and sour stock", 0, 300, "../../Assets/Images/Dish/vegClearSoup.png", 2, 26 },
                                new ArrayList{"Chicken Biriyani",345, "Richly flavored aromatic rice layered with succulent chicken in a delicate blend of whole spices; served along with raita and curry.",0, 205, "../../Assets/Images/Dish/chickenBiriyani.png", 1,26},
                                new ArrayList{"Mutton Biriyani",345, "Richly flavored aromatic rice layered with succulent mutton in a delicate blend of whole spices; served along with raita and curry.",0, 408, "../../Assets/Images/Dish/muttonBiriyani.png", 1,26},
                                new ArrayList{"Egg Biriyani",255, "Healthy yet wholesome boiled eggs covered in flavor-packed masala and slow cooked rice.",0, 90, "../../Assets/Images/Dish/eggBiriyani.png", 1,26},
                                new ArrayList{"Chicken Bucket Biriyani",1559, "4 to 5 portions of Thalappakatti Chicken Biryani served with 1 portion of Chicken 65, 4 boiled eggs, Raita & Gravy",0, 303, "../../Assets/Images/Dish/chickenBiriyani.png", 35,26},
                                new ArrayList{"Mutton Bucket Biriyani",1879, "4 to 5 portions of Thalappakatti Mutton Biryani served with 1 portion of Chicken 65, 4 boiled eggs, Raita & Gravy",0, 363, "../../Assets/Images/Dish/muttonBiriyani.png", 35,26},
                                new ArrayList{"Gulab Jamun",95, "Gulab Jamun. Soft. Plump and the way we are used to. There is nothing else in our gulab jamuns than authenticity and love.",1, 80, "../../Assets/Images/Dish/gulabJamun.png", 4,26},
                                new ArrayList{"Bread Halwa",55, "An Indian sweet made with bread cooked in ghee",1, 195, "../../Assets/Images/Dish/breadHalwa.png", 4,26},
                                new ArrayList{"Butter Milk",60, "Good old classic indian go to drink to keep the body cool and hydrated. Milk, curd and lemon churned in a pot to relish our thirs",1, 54, "../../Assets/Images/Dish/butterMilk.png", 7,26},
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Smoked Butter Chicken & Paratha Lunchbox",269, "Marinated tandoori chicken served in a mild but aromatic curry made is nourished with goodness by our master-chefs. Served with two warm & fresh Parathas. Sufficient for 1.",0,494, "../../Assets/Images/Dish/lunchB.png", 36,27},
                                new ArrayList{"Aloo Paratha With Dal Makhani", 189, "Two warm & fresh Aloo Paratha served with a comforting portion of creamy Dal Makhni..", 1,219, "../../Assets/Images/Dish/lunchB.png", 36,27},
                                new ArrayList{"Chicken Kheema & Bread Kulcha Lunchbox", 239, "Finely minced Chicken Kheema is slow-cooked to perfection with a lot of care and the perfect blend of spices. Served with two warm & fresh Bread Kulchas. Sufficient for one.", 0,42, "../../Assets/Images/Dish/lunchB.png", 36,27},
                                new ArrayList{"Rajma Masala Jumbo Lunchbox",259, "Fibre-rich kidney beans, slowly cooked in a thick and spicy onion tomato masala gravy. Served with a side of Basmati Rice, a homestyle Paratha and Gulab Jamun.", 1,24, "../../Assets/Images/Dish/lunchB.png", 36,27},
                                new ArrayList{"Masala Paneer Tikka Wrap",175, "Fresh paneer is smoked to perfection & drizzled with minty, spicy mayonnaise & wrapped in soft roti.",1, 111, "../../Assets/Images/Dish/wrap.png", 9,27},
                                new ArrayList{"Cheese Melt Chicken Bhuna Wrap",205, "Craving for something new? We got your back. We mean, we got you a wrap. Chunks of chicken cooked in bhuna masala, topped with sinful cheese into a soft roti.",0, 360, "../../Assets/Images/Dish/wrap.png", 9,27},
                                new ArrayList{"Cheesy Chicken Meatballs",129, "Juicy minced chicken meatballs, served with a cheesy mayonnaise dip - A no-brainer for snack cravings!",0, 129, "../../Assets/Images/Dish/meatballs.png", 10,27},
                                new ArrayList{"Potato Wedges",99, "When you dont know what to eat next we suggest keep it simple and call for the wedges.",1, 99, "../../Assets/Images/Dish/wedges.png", 10,27},
                                new ArrayList{"Coca - Cola",57, "Coca-Cola Bottle.",1, 155, "../../Assets/Images/Dish/cocaCola.png", 7,27},
                                new ArrayList{"Swig",59, "Aerated drink flavored with the mouth-puckering green apple. Sure to refresh you completely!",1, 55, "../../Assets/Images/Dish/swig.png", 7,27},
                                new ArrayList{"Gulab Jamun",95, "Gulab Jamun. Soft. Plump and the way we are used to. There is nothing else in our gulab jamuns than authenticity and love.",1, 80, "../../Assets/Images/Dish/gulabJamun.png", 4,27},
                                new ArrayList{"Bread Halwa",55, "An Indian sweet made with bread cooked in ghee",1, 195, "../../Assets/Images/Dish/breadHalwa.png", 4,27}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Biriyani",180, "Rice and veggies cooked in a fragrant and flavorful masala seasoned with indian whole spices. Served with salan and raita",1, 286, "../../Assets/Images/Dish/vegBiriyani.png", 5,28},
                                new ArrayList{"Pulao",206, "A delicious one pot Indian dish made with rice, vegetables, spices and herbs.",1, 178, "../../Assets/Images/Dish/pulao.png", 5,28},
                                new ArrayList{"Fried Rice",150, "An assortment of veggies and white rice tossed in a wok and spiced to perfection",1, 474, "../../Assets/Images/Dish/friedRice.png", 5,28},
                                new ArrayList{"Paneer Fried Rice",190, "An assortment of veggies, paneer and white rice tossed in a wok and spiced to perfection",1, 427, "../../Assets/Images/Dish/friedRice.png", 5,28},
                                new ArrayList{"Mushroom Fried Rice",180, "An assortment of veggies, mushroom and white rice tossed in a wok and spiced to perfection",1, 56, "../../Assets/Images/Dish/friedRice.png", 5,28},
                                new ArrayList{"Naan",50, "Soft flatbread grilled in tandoor and topped with butter.",1,62, "../../Assets/Images/Dish/naan.png", 37,28},
                                new ArrayList{"Butter Naan",60, "Soft flatbread grilled in tandoor.",1,96, "../../Assets/Images/Dish/naan.png", 37,28},
                                new ArrayList{"Paneer 65",220, "Deep-fried Paneer, marinated in a variety of spices and cooked to perfection.",1, 220, "../../Assets/Images/Dish/paneer65.png", 3,28},
                                new ArrayList{"Gobi 65",220, "Deep-fried Cauliflower, marinated in a variety of spices and cooked to perfection.",1, 220, "../../Assets/Images/Dish/paneer65.png", 3,28},
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Lemon Pepper Fried Chicken",125, "A crispy appetizer made of boneless chicken chunks, coated with batter and fried for a perfect golden colour.",0, 34, "../../Assets/Images/Dish/friedChicken.png", 3,29},
                                new ArrayList{"Mexican Fried Chicken",125, "A spicy appetizer made of boneless chicken chunks, coated with batter and fried for a perfect golden colour.",0, 379, "../../Assets/Images/Dish/friedChicken.png", 3,29},
                                new ArrayList{"Lemon Pepper Fried Chicken",125, "A crispy appetizer made of boneless chicken chunks, coated with batter and fried for a perfect golden colour.",0, 34, "../../Assets/Images/Dish/friedChicken.png", 3,29},
                                new ArrayList{"Mexican Fried Chicken",125, "A spicy appetizer made of boneless chicken chunks, coated with batter and fried for a perfect golden colour.",0, 379, "../../Assets/Images/Dish/friedChicken.png", 3,29},
                                new ArrayList{"Paneer Pizza",230, "A combination of juicy Paneer & 100% mozzarella cheese with flavourful signature pan sauce.",1, 91, "../../Assets/Images/Dish/pizza.png", 38,29},
                                new ArrayList{"Chicken Pizza",245, "A combination of juicy Chicken & 100% mozzarella cheese with flavourful signature pan sauce.",0, 291, "../../Assets/Images/Dish/pizza.png", 38,29},
                                new ArrayList{"Veg Burger",125, "An Iconic burger with Veggie Double patty, crispy shredded lettuce, internationally sourced iconic sauce and Gherkins.",1, 198, "../../Assets/Images/Dish/burger.png", 8,29},
                                new ArrayList{"Fried Chicken Burger",135, "An Iconic burger with Chicken Double patty, crispy shredded lettuce, internationally sourced iconic sauce and Gherkins.",0, 424, "../../Assets/Images/Dish/burger.png", 8,29},
                                new ArrayList{"Paneer Wrap",150, "Fresh paneer is smoked to perfection & drizzled with minty, spicy mayonnaise & wrapped in soft roti.",1, 111, "../../Assets/Images/Dish/wrap.png", 9,29},
                                new ArrayList{"Chicken Tikka Wrap",150, "Fresh Chicken is smoked to perfection & drizzled with minty, spicy mayonnaise & wrapped in soft roti.",0, 482, "../../Assets/Images/Dish/wrap.png", 9,29},
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Veg Clear Soup", 207, "A simple soup of assorted vegetables cooked in a lightly flavoured stock.", 1, 200, "../../Assets/Images/Dish/vegClearSoup.png", 2, 30 },
                                new ArrayList{"Chicken Clear Soup", 286, "A delectable soup prepared by simmering chicken slivers in a delightful hot and sour stock", 0, 300, "../../Assets/Images/Dish/vegClearSoup.png", 2, 30},
                                new ArrayList{"Paneer 65",265, "Deep-fried Paneer, marinated in a variety of spices and cooked to perfection.",1, 220, "../../Assets/Images/Dish/paneer65.png", 3,30},
                                new ArrayList{"Chicken 65",310, "Chicken 65 is a popular deep fried spicy chicken appetizer served with fried curry leaves roundels onion and lemon wages",0, 64, "../../Assets/Images/Dish/chicken65.png", 3,30},
                                new ArrayList{"Pepper Chicken",340, "Fried chicken in spicy brown gravy laced with lime for tanginess, coated with flavours from crushed pepper corns and spices.",0, 212, "../../Assets/Images/Dish/pepperChicken.png", 3,30},
                                new ArrayList{"Veg Burger",214, "An Iconic burger with Veggie Double patty, crispy shredded lettuce, internationally sourced iconic sauce and Gherkins.",1, 198, "../../Assets/Images/Dish/burger.png", 8,30},
                                new ArrayList{"Fried Chicken Burger",239, "An Iconic burger with Chicken Double patty, crispy shredded lettuce, internationally sourced iconic sauce and Gherkins.",0, 424, "../../Assets/Images/Dish/burger.png", 8,30},
                                new ArrayList{"French Fries",195, "Jazz Up Your Meal With Crispy Fries!",1, 234, "../../Assets/Images/Dish/fries.png", 16,30},
                                new ArrayList{"Chicken Nuggets",195, "Grilled minced chicken inspired from Persian flavors. Served with mayo dip.",0,364,"../../Assets/Images/Dish/nuggets.png",16,30},
                                new ArrayList{"Pepsi",57.14,"A Soft Drink.",1,52, "../../Assets/Images/Dish/pepsi.png", 7,30},
                                new ArrayList{"Mirinda",57.14, "A Soft Drink.",1, 497, "../../Assets/Images/Dish/mirinda.png", 7,30}
                            },
                            new List<ArrayList>
                            {
                                new ArrayList{"Butterscotch Pastry",159, "Homemade butterscotch with sponge cake.", 0,233,"../../Assets/Images/Dish/pastries.png",29,31},
                                new ArrayList{"Chocolate Truffle Pastry",159, " Layers of soft chocolate sponge and dense but silky-smooth chocolate ganache make this cake a decadent celebration.", 0,100,"../../Assets/Images/Dish/pastries.png",29,31},
                                new ArrayList{"New York Cheese Cake",309, " This New York Style Cheesecake, made with premium quality cream cheese and a crust infused with delicious mango flavor, is every dessert lovers delight!",1, 179, "../../Assets/Images/Dish/cheeseCake.png", 39,31},
                                new ArrayList{"Blueberry Cheese Cake",189, " This New York Style Blueberry Cheesecake, made with premium quality cream cheese and a crust infused with delicious mango flavor, is every dessert lovers delight!",1, 204, "../../Assets/Images/Dish/cheeseCake.png", 39,31},
                                new ArrayList{"Mango Cheese Cake",169, " This New York Style Mango Cheesecake, made with premium quality cream cheese and a crust infused with delicious mango flavor, is every dessert lovers delight!",1, 262, "../../Assets/Images/Dish/cheeseCake.png", 39,31},
                                new ArrayList{"Coca - Cola",57, "Coca-Cola Bottle.",1, 155, "../../Assets/Images/Dish/cocaCola.png", 7,31},
                                new ArrayList{"Swig",59, "Aerated drink flavored with the mouth-puckering green apple. Sure to refresh you completely!",1, 55, "../../Assets/Images/Dish/swig.png", 7,31}
                            },
                        };

                        ArrayList cuisineGUIDs = new ArrayList();
                        ArrayList dishCategoryGUIDs = new ArrayList();
                        ArrayList restaurantGUIDs = new ArrayList();

                        for (int i = 0; i < cuisines.Length; i++)
                        {
                            // ADDING CUISINES TO DB
                            cuisineGUIDs.Add(Guid.NewGuid().ToString());
                            command.CommandText = "INSERT INTO cuisineCategories VALUES($id,$category);";
                            command.Parameters.AddWithValue("$category", cuisines[i]);
                            command.Parameters.AddWithValue("$id", cuisineGUIDs[i]);
                            command.ExecuteNonQuery();
                            command.Parameters.RemoveAt("$category");
                            command.Parameters.RemoveAt("$id");
                        }

                        for (int i = 0; i < dishCategories.Length; i++)
                        {
                            // ADDING DISH CATEGORIES TO DB
                            dishCategoryGUIDs.Add(Guid.NewGuid().ToString());
                            command.CommandText = "INSERT INTO dishCategories VALUES($id,$category);";
                            command.Parameters.AddWithValue("$category", dishCategories[i]);
                            command.Parameters.AddWithValue("$id", dishCategoryGUIDs[i]);
                            command.ExecuteNonQuery();
                            command.Parameters.RemoveAt("$category");
                            command.Parameters.RemoveAt("$id");
                        }

                        for (int i = 0; i < restaurantName.Length; i++)
                        {
                            // ADDING HOTELS TO DB
                            restaurantGUIDs.Add(Guid.NewGuid().ToString());
                            command.CommandText = "INSERT INTO restaurants VALUES($id,$name,'','',$costForTwo,$imagePath);";
                            command.Parameters.AddWithValue("$id", restaurantGUIDs[i]);
                            command.Parameters.AddWithValue("$name", restaurantName[i]);
                            command.Parameters.AddWithValue("$imagePath", restaurantImagePath[i]);
                            command.Parameters.AddWithValue("$costForTwo", costForTwo[i]);
                            command.ExecuteNonQuery();
                            command.Parameters.RemoveAt("$name");
                            command.Parameters.RemoveAt("$imagePath");
                            command.Parameters.RemoveAt("$costForTwo");
                            command.Parameters.RemoveAt("$id");

                            // LINKING CUISINES TO HOTELS IN DB

                            for (int j = 0; j < restaurantCuisines[i].Count; j++)
                            {
                                command.CommandText = "INSERT INTO cuisines VALUES($cuisineCategoryId,$restaurantId);";
                                command.Parameters.AddWithValue("$cuisineCategoryId", cuisineGUIDs[restaurantCuisines[i][j] - 1]);
                                command.Parameters.AddWithValue("$restaurantId", restaurantGUIDs[i]);
                                command.ExecuteNonQuery();
                                command.Parameters.RemoveAt("$cuisineCategoryId");
                                command.Parameters.RemoveAt("$restaurantId");
                            }

                            // ADDING DISHES IN DB

                            for (int j = 0; j < dishes[i].Count; j++)
                            {
                                command.CommandText = "INSERT INTO dishes VALUES($id,$name,$cost,$description,$isVeg,$soldCount,$imagePath,$dishCategoryId,$restaurantId);";

                                command.Parameters.AddWithValue("$id", Guid.NewGuid().ToString());
                                command.Parameters.AddWithValue("$name", dishes[i][j][0]);
                                command.Parameters.AddWithValue("$cost", dishes[i][j][1]);
                                command.Parameters.AddWithValue("$description", dishes[i][j][2]);
                                command.Parameters.AddWithValue("$isVeg", dishes[i][j][3]);
                                command.Parameters.AddWithValue("$soldCount", dishes[i][j][4]);
                                command.Parameters.AddWithValue("$imagePath", dishes[i][j][5]);
                                command.Parameters.AddWithValue("$dishCategoryId", dishCategoryGUIDs[((int)dishes[i][j][6]) - 1]);
                                command.Parameters.AddWithValue("$restaurantId", restaurantGUIDs[((int)dishes[i][j][7]) - 1]);
                                command.ExecuteNonQuery();
                                command.Parameters.RemoveAt("$name");
                                command.Parameters.RemoveAt("$cost");
                                command.Parameters.RemoveAt("$description");
                                command.Parameters.RemoveAt("$isVeg");
                                command.Parameters.RemoveAt("$soldCount");
                                command.Parameters.RemoveAt("$imagePath");
                                command.Parameters.RemoveAt("$dishCategoryId");
                                command.Parameters.RemoveAt("$restaurantId");
                                command.Parameters.RemoveAt("$id");
                            }
                        }

                    }
                }
            }
        }
    }
}
