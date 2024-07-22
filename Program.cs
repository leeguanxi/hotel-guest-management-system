//========================================================== 
// Student Number : S10244222
// Student Name : Xavier Ng
//========================================================== 

//========================================================== 
// Student Number : S10244223
// Student Name : Lee Guan Xi
//========================================================== 

using P08_Team9_PRG2Assignment;

// Exception handling do as last.
List<Room> roomList = new List<Room>();
List<Guest> guestList = new List<Guest>();
InitRoomData(roomList);
InitGuestData(guestList, roomList);

while (true)
{
    try
    {
        DisplayMenu();
        Console.Write("Enter option: ");
        int option = Convert.ToInt32(Console.ReadLine());

        // Basic Feature 1 - List all guest information
        if (option == 1)
        {
            DisplayGuest(guestList);
        }

        // Basic Feature 2 - List all available rooms
        else if (option == 2)
        {
            DisplayAvailRooms(roomList);
        }

        // Basic Feature 3 - Register Guest
        else if (option == 3)
        {
            RegisterGuest(guestList);
        }

        // Basic Feature 4 - Check-in Guest
        else if (option == 4)
        {
            CheckInGuest();
        }

        // Basic Feature 5 - Display stay details of a guest
        else if (option == 5)
        {
            StayDetails(guestList);
        }

        // Basic Feature 6 - Extends the stay by number of day
        else if (option == 6)
        {
            ExtendStay();
        }

        // Advanced Feature A - Display monthly charged amounts breakdowns & total charged amounts for the year
        else if (option == 7)
        {
            MonthlyCharges();
        }

        // Advanced Feature B - Check out guest
        else if (option == 8)
        {
            CheckoutGuest();
        }

        // Exit
        else if (option == 0)
        {
            Console.WriteLine("Quit successfully.");
            break;
        }
        else
        {
            Console.WriteLine("Invalid option!");
        }
    }
    catch
    {
        Console.WriteLine("Invalid option!");
        continue;
    }
}

// METHODS

void DisplayMenu()
{
    Console.WriteLine("//////////////////////////////////////\r\n||      GUEST MANAGEMENT SYSTEM     || \r\n//////////////////////////////////////\r\n================ MENU ================\r\n\r\n[1] List all guest\r\n[2] List all available rooms\r\n[3] Register guest\r\n[4] Check-in guest\r\n[5] Display guest's stay details\r\n[6] Extend stay\r\n[7] Display monthly charge\r\n[8] Check out guest\r\n[0] Exit\r\n\r\n======================================");
}

// creates Guest object with stay details, room details and membership details
void InitGuestData(List<Guest> guestList, List<Room> roomList) // store guest objects into list
{
    string[] guestsLines = File.ReadAllLines("Guests.csv");
    for (int i = 1; i < guestsLines.Length; i++)
    {
        string[] guestData = guestsLines[i].Split(",");
        Membership membership = new Membership(guestData[2], Convert.ToInt32(guestData[3]));

        // stay details into guest object
        string[] staysLines = File.ReadAllLines("Stays.csv");
        for (int j = 0; j < staysLines.Length; j++)
        {
            string[] staysData = staysLines[j].Split(",");
            if (guestData[1] == staysData[1]) // passportnum == passportnum
            {
                Stay stay = new Stay(Convert.ToDateTime(staysData[3]), Convert.ToDateTime(staysData[4]));
                Guest guest = new Guest(guestData[0], guestData[1], stay, membership);
                guest.IsCheckedIn = str2bool(staysData[2]);

                // adds which Room belongs to which Guest
                try
                {
                    Room room1 = SearchRoom(Convert.ToInt32(staysData[9])); // second room
                    Addons(room1, staysData[10], staysData[11], staysData[12]); // addons: wifi, breakfast, extra bed
                    stay.AddRoom(room1);

                    Room room2 = SearchRoom(Convert.ToInt32(staysData[5])); // first room
                    Addons(room2, staysData[6], staysData[7], staysData[8]);
                    stay.AddRoom(room2); // adds to roomlist in Stay.cs
                }
                catch (Exception ex) // runs if guest has no 2nd room
                {
                    Room room1 = SearchRoom(Convert.ToInt32(staysData[5]));
                    Addons(room1, staysData[6], staysData[7], staysData[8]);
                    guest.HotelStay.AddRoom(room1); // adds to roomlist in Stay.cs
                }

                guestList.Add(guest);
            }
        }

    }
}

// Updates the Guest's addons
void Addons(Room room, string wifi, string breakfast, string bed)
{
    if (room is StandardRoom)
    {
        StandardRoom r = (StandardRoom)room;
        r.requireWifi = str2bool(wifi);
        r.requireBreakfast = str2bool(breakfast);
    }
    else
    {

        DeluxeRoom r = (DeluxeRoom)room;
        r.additionalBed = str2bool(bed);
    }
}

// checks addons true or false (convert str to bool)
bool str2bool(string boolean)
{
    if (boolean == "TRUE")
    {
        return true;
    }
    else return false;
}

// display guest info (name, passportno, memberstatus, points)
void DisplayGuest(List<Guest> guestList)
{
    Console.WriteLine("\nGuests:");
    Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -15}", "Name", "PassportNo", "Member Status", "Points");
    foreach (Guest guest in guestList)
    {
        Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -15}", guest.Name, guest.PassportNum, guest.Member.Status, guest.Member.Points);
    }
}

// stores room objects into roomList
void InitRoomData(List<Room> roomList)
{

    string[] roomlines = File.ReadAllLines("Rooms.csv");
    for (int i = 1; i < roomlines.Length; i++)
    {
        string[] info = roomlines[i].Split(',');
        int rn = Convert.ToInt32(info[1]);
        string bc = info[2];
        double dr = Convert.ToDouble(info[3]);
        bool ia = RoomAvail(rn);

        if (info[0] == "Standard")
        {
            StandardRoom room = new StandardRoom(rn, bc, dr, ia);
            roomList.Add(room);
        }
        else if (info[0] == "Deluxe")
        {
            DeluxeRoom room = new DeluxeRoom(rn, bc, dr, ia);
            roomList.Add(room);
        }
    }
}

// display all available rooms (rooms not checked in)
void DisplayAvailRooms(List<Room> roomList)
{
    Console.WriteLine("\nAvailable Rooms:");
    Console.WriteLine("{0, -15} {1, -15} {2, -20} {3, -15}", "Room Type", "Room Number", "Bed Configuration", "Daily Rate");
    foreach (Room room in roomList)
    {
        if (room.isAvail == true)
        {
            if (room is StandardRoom)
            {
                Console.WriteLine("{0, -15} {1, -15} {2, -20} {3, -15}", "Standard", room.roomNumber, room.bedConfiguration, room.dailyRate);
            }
            else
            {
                Console.WriteLine("{0, -15} {1, -15} {2, -20} {3, -15}", "Deluxe", room.roomNumber, room.bedConfiguration, room.dailyRate);
            }
        }
        else continue;
    }
}

// check whether room is checked in
bool RoomAvail(int rn)
{
    string[] csvLines = File.ReadAllLines("Stays.csv");
    for (int i = 1; i < csvLines.Length; i++)
    {
        string[] info = csvLines[i].Split(",");

        try
        {
            if (Convert.ToInt32(info[5]) == rn || Convert.ToInt32(info[9]) == rn) // RoomNum
            {
                if (info[2] == "TRUE") // IsCheckedIn
                {
                    return false; // room is NOT available
                }
            }
        }
        catch (Exception ex) // runs when there is no 2nd room
        {
            if (Convert.ToInt32(info[5]) == rn) // RoomNum
            {
                if (info[2] == "TRUE") // IsCheckedIn
                {
                    return false; // room is NOT available
                }
            }
        }

    }
    return true; // room is available
}

// searches room in roomList
Room SearchRoom(int rn)
{
    foreach (Room room in roomList)
    {
        if (rn == room.roomNumber)
        {
            return room;
        }
    }
    return null;
}

// creates new guest object and adding it to guestList, also appends to Guests.csv
void RegisterGuest(List<Guest> guestList)
{
    Console.Write("Enter name of guest: ");
    string name = Console.ReadLine();
    Console.Write("Enter passport number: ");
    string passport = Console.ReadLine().ToUpper();
    

    // check if input is empty
    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(passport))
    {
        Console.WriteLine("Error! Name and Passport Number cannot be empty! Please try again.\n");
    }
    else if (char.IsLetter(passport[0]) == false || char.IsLetter(passport[passport.Length-1]) == false || passport.Length != 9)
    {
        Console.WriteLine("Please enter a valid passport number!");
    }
    else
    {
        Membership member = new Membership("Ordinary", 0);
        Stay stay = new Stay();
        Guest newGuest = new Guest(name, passport, stay, member);
        guestList.Add(newGuest);

        // appending guest infomation to Guest.csv
        string guestData = $"{name},{passport},{member.Status},{member.Points}";
        File.AppendAllText("Guests.csv", guestData);

        Console.WriteLine("Guest added successfully!");
    }

}

// retrieves Guest's stay details and room info from guestList
void StayDetails(List<Guest> guestList)
{
    int count = 1;
    Console.WriteLine("{0, -5} {1, -15} {2, -15}", "No.", "Name", "Passport Num");
    foreach (Guest guest in guestList)
    {
        Console.WriteLine("{0, -5} {1, -15} {2, -15}", count, guest.Name, guest.PassportNum);        
        count++;
    }
    Console.Write("\nEnter no. : ");
    int option = Convert.ToInt32(Console.ReadLine());

    if (option > count || option < 1)
    { 
        Console.WriteLine("Error! Guest not found.");
    }
    else
    {
        Guest guest = guestList[option - 1];
        List<Room> rooms = guest.HotelStay.roomList;
        Console.WriteLine("\nName: " + guest.Name);
        Console.WriteLine("Passport Num: " + guest.PassportNum);
        Console.WriteLine("Check In: " + guest.HotelStay.CheckinDate.ToString("dd/MM/yyyy"));
        Console.WriteLine("Check Out: " + guest.HotelStay.CheckoutDate.ToString("dd/MM/yyyy"));
        Console.WriteLine("\n{0, -10} {1, -15} {2, -15} {3, -10} {4, -10} {5, -10}", "Room No.", "Room Type", "Bed Config.", "Wifi", "Breakfast", "Extra Bed");
        foreach (Room room in rooms)
        {
            if (room is StandardRoom)
            {
                StandardRoom r = (StandardRoom)room;
                string wifi = bool2str(r.requireWifi);
                string breakfast = bool2str(r.requireBreakfast);
                Console.WriteLine("{0, -10} {1, -15} {2, -15} {3, -10} {4, -10} {5, -10}", room.roomNumber, "Standard", room.bedConfiguration, wifi, breakfast, "N.A.");
            }
            else
            {
                DeluxeRoom r = (DeluxeRoom)room;
                string bed = bool2str(r.additionalBed);
                Console.WriteLine("{0, -10} {1, -15} {2, -15} {3, -10} {4, -10} {5, -10}", room.roomNumber, "Deluxe", room.bedConfiguration, "N.A.", "N.A.", bed);
            }
        }
    }
}

// converts bool to str
string bool2str(bool boolean)
{
    if (boolean == true)
    {
        return "YES";
    }
    else
    {
        return "NO";
    }
}

//Feature 4 Pt 1
int CheckInGuest1(Stay checkinstay)
{
    Console.WriteLine("\nAvailable Rooms:");
    Console.WriteLine("{0, -15} {1, -15} {2, -20} {3, -15} {4,-15}", "No.", "Room Type", "Room Number", "Bed Configuration", "Daily Rate");
    int count1 = 1;
    List<Room> availlist = new List<Room>();
    foreach (Room room in roomList)
    {
        if (room.isAvail == true)
        {
            if (room is StandardRoom)
            {
                Console.WriteLine("{0, -15} {1, -15} {2, -20} {3, -15} {4,-15}", count1, "Standard", room.roomNumber, room.bedConfiguration, room.dailyRate);
                availlist.Add(room);
            }
            else
            {
                Console.WriteLine("{0, -15} {1, -15} {2, -20} {3, -15} {4,-15}", count1, "Deluxe", room.roomNumber, room.bedConfiguration, room.dailyRate);
                availlist.Add(room);
            }
            count1++;
        }
        else continue;
    }
    Console.Write("Enter a room:");
    int roomnum = Convert.ToInt32(Console.ReadLine());
    if (roomnum > count1 || roomnum < 1)
    {
        Console.WriteLine("Error! You did not choose a room");
        return 0;
    }
    else
    {
        string? wifi = "N";
        string? bkfast = "N";
        string? addbed = "N";
        if (availlist[roomnum - 1] is StandardRoom)
        {
            Console.Write("Require Wifi?(Y/N):");
            wifi = Console.ReadLine();
            if (wifi.ToUpper() != "Y" & wifi.ToUpper() != "N")
            {
                Console.WriteLine("Error!You did not choose the options.");
                return 0;
            }
            else
            {
                Console.Write("Require Breakfast?(Y/N):");
                bkfast = Console.ReadLine();
                if (bkfast.ToUpper() != "Y" & bkfast.ToUpper() != "N")
                {
                    Console.WriteLine("Error!You did not choose the options.");
                    return 0;
                }
                else
                {
                    string? wifi1 = wifi.ToUpper();
                    string? bkfast1 = bkfast.ToUpper();
                    if (wifi1 == "Y" & bkfast1 == "Y")
                    {
                        StandardRoom r = (StandardRoom)availlist[roomnum - 1];
                        r.requireWifi = true;
                        r.requireBreakfast = true;
                    }
                    else if (wifi1 == "N" & bkfast1 == "Y")
                    {
                        StandardRoom r = (StandardRoom)availlist[roomnum - 1];
                        r.requireBreakfast = true;
                        r.requireWifi = false;
                    }
                    else if (wifi1 == "Y" & bkfast1 == "N")
                    {
                        StandardRoom r = (StandardRoom)availlist[roomnum - 1];
                        r.requireWifi = true;
                        r.requireBreakfast = false;
                    }
                    else
                    {
                        StandardRoom r = (StandardRoom)availlist[roomnum - 1];
                        r.requireBreakfast = false;
                        r.requireWifi = false;
                    }
                }
            }
        }
        else
        {
            Console.Write("Additional Bed?(Y/N):");
            addbed = Console.ReadLine();
            if (addbed.ToUpper() != "Y" & addbed.ToUpper() != "N")
            {
                Console.WriteLine("Error! You did not choose the options.");
                return 0;
            }
            else
            {
                if (addbed.ToUpper() == "Y")
                {
                    DeluxeRoom r = (DeluxeRoom)availlist[roomnum - 1];
                    r.additionalBed = true;
                }
            }

        }
        availlist[roomnum - 1].isAvail = false;
        checkinstay.roomList.Add(availlist[roomnum - 1]);
        return 1;
    }
}

//Feature 4 Pt 2
void CheckInGuest()
{
    int count = 1;
    Console.WriteLine("{0, -5} {1, -15} {2, -15}", "No.", "Name", "Passport Num");
    int option = 0;
    foreach (Guest guest in guestList)
    {
        Console.WriteLine("{0, -5} {1, -15} {2, -15}", count, guest.Name, guest.PassportNum);
        count++;
    }
    Console.Write("\nEnter no. : ");
    try
    {
        option = Convert.ToInt32(Console.ReadLine());

        if (option >= count || option < 1)
        {
            Console.WriteLine("Error! Guest not found.");
        }
        else if (guestList[option - 1].IsCheckedIn == true)
        {
            Console.WriteLine("Guest is already checked in.");
        }
        else
        {
            try
            {
                Console.Write("Enter check in date(yyyy/mm/dd):");
                DateTime checkin = Convert.ToDateTime(Console.ReadLine());
                if (checkin.Subtract(DateTime.Now).Days < 0)
                {
                    Console.WriteLine("Error! Invalid Check In date.");
                }
                else
                {
                    Console.Write("Enter check out date(yyyy/mm/dd):");
                    DateTime checkout = Convert.ToDateTime(Console.ReadLine());
                    if (checkout.Subtract(checkin).Days <= 0)
                    {
                        Console.WriteLine("Error! Invalid Check Out date.");
                    }
                    else
                    {
                        Stay checkinstay = new Stay(checkin, checkout);
                        try
                        {
                            int num = CheckInGuest1(checkinstay);
                            if (num == 1)
                            {
                                List<Room> availlist = new List<Room>();
                                foreach(Room r in roomList)
                                {
                                    if (r.isAvail == true)
                                    {
                                        availlist.Add(r);
                                    }
                                }
                                while (true)
                                {
                                    Console.Write("Select another room?(Y/N):");
                                    string? addroom = Console.ReadLine();
                                    if (addroom.ToUpper() == "N")
                                    {
                                        break;
                                    }
                                    else if (addroom.ToUpper() == "Y")
                                    {
                                        Console.Write("How many additional rooms?(Max = {0}):", availlist.Count());
                                        int numrooms = Convert.ToInt32(Console.ReadLine());
                                        if (numrooms < 1 || numrooms > availlist.Count())
                                        {
                                            Console.WriteLine("Error!Choose below the max. ");
                                        }
                                        else
                                        {
                                            for(int i = 1; i <= numrooms; i++)
                                            {
                                                Console.WriteLine("Room {0}:", i);
                                                CheckInGuest1(checkinstay);
                                            }
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Did you choose a option?");
                                    }
                                }
                                guestList[option - 1].HotelStay = checkinstay;
                                guestList[option - 1].IsCheckedIn = true;
                                Console.WriteLine("Check in successful.");
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Error!");
                        }
                    }
                }

            }
            catch
            {
                Console.WriteLine("Error! Check in date not entered correctly.");
            }
        }
    }
    catch
    {
        Console.WriteLine("Error!Choose a number.");
    }
}

void MonthlyCharges()
{
    List<string> month = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    Console.Write("Enter the year: ");
    int year = Convert.ToInt32(Console.ReadLine());

    double total = 0;
    for ( int i = 1; i <= 12; i++)
    {
        double monthCharge = 0;
        foreach (Guest guest in guestList)
        {
            if (guest.HotelStay.CheckoutDate.Year == year)
            {
                if (guest.HotelStay.CheckoutDate.Month == i)
                {
                    monthCharge += guest.HotelStay.CalculateTotal();                  
                }
            }
        }
        total += monthCharge;
        Console.WriteLine("{0} {1, -10}: ${2:0.00}", month[i-1], year, monthCharge);
    }
    Console.WriteLine("\nTotal: ${0:0.00}", total);
}

//Feature 6
void ExtendStay()
{
    int count = 1;
    foreach (Guest guest in guestList)
    {
        Console.WriteLine("{0, -5} {1, -15} {2, -15}", count, guest.Name, guest.PassportNum);
        count++;
    }
    int option = 0;
    Console.Write("Enter guest:");
    try 
    {
        option = Convert.ToInt32(Console.ReadLine());
    }
    catch
    {
        Console.WriteLine("Error! You did not pick one of the options");
    }
    if (guestList[option - 1].IsCheckedIn == true)
    {
        try
        {
            Console.Write("No. of days:");
            int days = Convert.ToInt32(Console.ReadLine());
            if (days < 1)
            {
                Console.WriteLine("Error!Choose correctly.");
            }
            else
            {
                Stay g = guestList[option - 1].HotelStay;
                g.CheckoutDate = g.CheckoutDate.AddDays(days);
                Console.WriteLine("Checkout date updated");
            }
        }
        catch
        {
            Console.WriteLine("Error! You did not choose correctly.");
        }
    }
    else
    {
        Console.WriteLine("Guest is not checked in.");
    }    

}

void CheckoutGuest()
{
    int count = 1;
    foreach (Guest guest in guestList)
    {
        Console.WriteLine("{0, -5} {1, -15} {2, -15}", count, guest.Name, guest.PassportNum);
        count++;
    }
    int option = 0;
    Console.Write("Enter guest:");
    try
    {
        option = Convert.ToInt32(Console.ReadLine());
        if (option < 1 || option >= count)
        {
            Console.WriteLine("Error!Choose an option.");
        }
        else if (guestList[option - 1].IsCheckedIn == false)
        {
            Console.WriteLine("Guest is already checked out.");
        }
        else
        {
            
            Guest guest = guestList[option - 1];
            Console.WriteLine("Total bill: ${0}", guest.HotelStay.CalculateTotal());
            Console.WriteLine("Membership status: {0}", guest.Member.Status);
            Console.WriteLine("Points: {0}", guest.Member.Points);
            if (guest.Member.Status == "Ordinary")
            {
                Console.WriteLine("Points are unable to be used.");
                double totalcharges = guest.HotelStay.CalculateTotal();
                Console.WriteLine("Final bill: ${0}", totalcharges);
                Console.Write("Press any key for payment: ");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key == key)
                {
                    guest.Member.EarnPoints(totalcharges);
                    guest.IsCheckedIn = false;
                    foreach (Room r in guest.HotelStay.roomList)
                    {
                        r.isAvail = true;
                    }
                    Console.WriteLine();
                    Console.WriteLine("Checkout done.");
                }
                else
                {
                    Console.WriteLine("No Key Pressed.");
                }
            }
            else
            {
                while (true)
                {
                    try
                    {
                        Console.Write("How many points will be used:");
                        int pointuse = Convert.ToInt32(Console.ReadLine());
                        if (pointuse > guest.Member.Points || pointuse < 0)
                        {
                            Console.WriteLine("Error!Not enough points.");
                            continue;
                        }
                        else
                        {
                            guest.Member.RedeemPoints(pointuse);
                            double totalcharges = guest.HotelStay.CalculateTotal() - Convert.ToDouble(pointuse);
                            Console.WriteLine("Final bill after redeeming points: ${0}", totalcharges);
                            Console.Write("Press any key for payment: ");
                            ConsoleKeyInfo key = Console.ReadKey();
                            if (key == key)
                            { 
                                guest.Member.EarnPoints(totalcharges);
                                guest.IsCheckedIn = false;
                                foreach(Room r in guest.HotelStay.roomList)
                                {
                                    r.isAvail = true;
                                }
                                Console.WriteLine();
                                Console.WriteLine("Checkout done.");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("No Key Pressed.");
                                break;
                            }

                        }

                    }
                    catch
                    {
                        Console.WriteLine("Error! Please use integers.");
                    }
                }
            }
        }
    }
    catch
    {
        Console.WriteLine("Error! You did not pick one of the options");
    }
}

//
//                       _oo0oo_
//                      o8888888o
//                      88" . "88
//                      (| -_- |)
//                      0\  =  /0
//                    ___/`---'\___
//                  .' \\|     |// '.
//                 / \\|||  :  |||// \
//                / _||||| -:- |||||- \
//               |   | \\\  -  /// |   |
//               | \_|  ''\---/''  |_/ |
//               \  .-\__  '-'  ___/-. /
//             ___'. .'  /--.--\  `. .'___
//          ."" '<  `.___\_<|>_/___.' >' "".
//         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//         \  \ `_.   \_ __\ /__ _/   .-` /  /
//     =====`-.____`.___ \_____/___.-`___.-'=====
//                       `=---='
//
//
//     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//               佛祖保佑         没有BUG
//
