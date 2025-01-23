using System.Diagnostics.Metrics;
using static PATHFORMING_IV.InputForm;

namespace PATHFORMING_IV
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }


        public static Graph school = new Graph(197);
        public static string pathways = "";

        public void InputForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(960, 540);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            Label InitialRoomLabel = new Label();
            Label EndRoomLabel = new Label();
            Button GoButton = new Button();
            Button MapButton = new Button();
            Label DirectiosnOutput = new Label();
            TextBox InitialRoomInput = new TextBox();
            TextBox EndRoomInput = new TextBox();

            InitialRoomLabel.Location = new Point(30, 27);
            InitialRoomLabel.Name = "InitialRoomLabel";
            InitialRoomLabel.AutoSize = true;
            InitialRoomLabel.Text = "Room you are in";
            InitialRoomLabel.Font = new Font("Sitka Text", 9.749999F, FontStyle.Regular, GraphicsUnit.Point);

            EndRoomLabel.Location = new Point(170, 27);
            EndRoomLabel.Name = "EndRoomLabel";
            EndRoomLabel.AutoSize = true;
            EndRoomLabel.Text = "Room you want to get to";
            EndRoomLabel.Font = new Font("Sitka Text", 9.749999F, FontStyle.Regular, GraphicsUnit.Point);

            GoButton.Location = new Point(360, 75);
            GoButton.Name = "GoButton";
            GoButton.Size = new Size(75, 28);
            GoButton.Text = "GO";
            GoButton.Click += GoButton_Clicked;
            GoButton.Font = new Font("Sitka Text", 9F, FontStyle.Regular, GraphicsUnit.Point);

            MapButton.Location = new Point(360, 20);
            MapButton.Name = "MapButton";
            MapButton.Size = new Size(75, 28);
            MapButton.Text = "Map";
            MapButton.Click += MapButton_Clicked;
            MapButton.Font = new Font("Sitka Text", 9F, FontStyle.Regular, GraphicsUnit.Point);

            DirectiosnOutput.Location = new Point(500, 75);
            DirectiosnOutput.Name = "DirectiosnOutput";
            DirectiosnOutput.AutoSize = true;
            DirectiosnOutput.Text = "";
            DirectiosnOutput.Font = new Font("Sitka Text", 9.749999F, FontStyle.Regular, GraphicsUnit.Point);

            InitialRoomInput.Location = new Point(30, 75);
            InitialRoomInput.Name = "InitialRoomInput";
            InitialRoomInput.Size = new Size(100, 23);
            InitialRoomInput.Leave += textBox_TextChanged;
            InitialRoomInput.Font = new Font("Sitka Text", 9.749999F, FontStyle.Regular, GraphicsUnit.Point);

            EndRoomInput.Location = new Point(170, 75);
            EndRoomInput.Name = "EndRoomInput";
            EndRoomInput.Size = new Size(150, 23);
            EndRoomInput.Leave += textBox_TextChanged;
            EndRoomInput.Font = new Font("Sitka Text", 9.749999F, FontStyle.Regular, GraphicsUnit.Point);

            Controls.Add(InitialRoomLabel);
            Controls.Add(EndRoomLabel);
            Controls.Add(InitialRoomInput);
            Controls.Add(EndRoomInput);
            Controls.Add(DirectiosnOutput);
            Controls.Add(GoButton);
            Controls.Add(MapButton);

            StreamReader apple = new StreamReader("U:/connections.txt");
            List<string> tree = new List<string>();   //makes list with <datatype> name = new List <datatype>()
            do
            {
                tree.Add(apple.ReadLine());
            } while (!apple.EndOfStream);

            school.addconnectionbulk(tree);

            StreamReader olive = new StreamReader("U:/roomnames.txt");
            List<string> branch = new List<string>();   //makes list with <datatype> name = new List <datatype>()
            do
            {
                branch.Add(olive.ReadLine());
            } while (!olive.EndOfStream);

            school.addroomnamebulk(branch);

            //will be replaced with stuf from actual graph
            List<string> roomnames = school.roomnamesgrab();
        }

        public class Graph
        {
            private int rooms;
            private List<Tuple<int, int>>[] connections; //using tuple so list has 3 effective parts, room 1 (index), room 2(tuple item 1) and the distance between them (tuple item 2)
                                                         //side note but tuplets are so weird i wish i used a different metod of doing this but whatever it works
            private List<string> roomnames = new List<string>();

            public Graph(int rooms)
            {
                this.rooms = rooms;
                connections = new List<Tuple<int, int>>[rooms];
                for (int i = 0; i < rooms; i++)
                {
                    //connections[i] = new Tuple<int, int>();
                    connections[i] = new List<Tuple<int, int>>(); //doesnt accept tuple on its own even tho its a list of tuples
                }
            }

            public void addconnection(int roomuno, int roomdos, int distance)
            {
                connections[roomuno].Add(new Tuple<int, int>(roomdos, distance));
                connections[roomdos].Add(new Tuple<int, int>(roomuno, distance)); //so theyre not one way
            }
            public void addconnectionbulk(List<string> strings)
            {
                for (int i = 0; i < strings.Count; i++)
                {
                    string[] substring = strings[i].Split(',');
                    int[] subint = { Convert.ToInt32(substring[0]), Convert.ToInt32(substring[1]), Convert.ToInt32(substring[2]) };
                    connections[subint[0]].Add(new Tuple<int, int>(subint[1], subint[2]));
                    connections[subint[1]].Add(new Tuple<int, int>(subint[0], subint[2])); //so theyre not one way
                }
            }
            public void addconnectiononeway(int roomuno, int roomdos, int distance)
            {
                connections[roomuno].Add(new Tuple<int, int>(roomdos, distance)); //so they are one way :P
            }
            public void addroomname(string roomname)
            {
                roomnames.Add(roomname);
            }
            public void addroomnamebulk(List<string> strings)
            {
                foreach (string s in strings)
                {
                    roomnames.Add(s);
                }
            }
            public List<string> roomnamesgrab()
            {
                return roomnames;
            }
            public List<Tuple<int, int>>[] connectionsgrab()
            {
                return connections;
            }
        }

        public class ActualPathFindingAlgorithm
        {
            public static int[] dijkstra(Graph graph, int destination, int usersroom, List<string> roomnames)
            {
                int rooms = graph.connectionsgrab().Length;
                bool[] possiblequickestroute = new bool[rooms];
                int[] shortestdistances = new int[rooms];
                string[] pathway = new string[rooms];
                //List<Tuple<int, int>>[,] alsopathway = new List<Tuple<int, int>>[,];        

                for (int i = 0; i < rooms; i++)
                {
                    possiblequickestroute[i] = false;
                    pathway[i] += Convert.ToString(i);
                    shortestdistances[i] = int.MaxValue; //ironic, set to as close to infinite as to reassign later easily
                }
                shortestdistances[destination] = 0;
                for (int i = 0; i < rooms; i++)
                {
                    int nextshortest = MinimumDistance(shortestdistances, possiblequickestroute);
                    //goes through each path shortest to longest
                    possiblequickestroute[nextshortest] = true;
                    //locks it in as the shortest possible
                    foreach (var connected in graph.connectionsgrab()[nextshortest]) //every connection to nextshortest
                    {
                        int otherroom = connected.Item1;
                        int distance = connected.Item2;
                        if (!possiblequickestroute[otherroom] && (shortestdistances[nextshortest] != int.MaxValue) && (shortestdistances[nextshortest] + distance < shortestdistances[otherroom])) //if you havent already found a route, if the distance has been reassigned and if the distance would be smaller than it already is
                        {
                            shortestdistances[otherroom] = shortestdistances[nextshortest] + distance;
                            pathway[otherroom] = Convert.ToString(otherroom) + "→" + Convert.ToString(nextshortest);
                        }
                    }

                    if (nextshortest == usersroom)
                    {
                        pathways = "";
                        int intemp = nextshortest;
                        string[] stemp;
                        while (intemp != destination)
                        {
                            //MessageBox.Show(pathway[intemp]);
                            stemp = pathway[intemp].Split('→');
                            for (int j = 0; j < stemp.Length; j++)
                            {
                                pathways += (roomnames[Convert.ToInt32(stemp[j])] + "(" + Convert.ToInt32(stemp[j]) + ")");
                                if (j != stemp.Length - 1)
                                {
                                    pathways += " → ";
                                }
                                else
                                {
                                    pathways += (" ");
                                }
                            }
                            intemp = Convert.ToInt32(stemp[stemp.Length - 1]);
                            pathways += "\r\n";
                        }

                    }
                }
                return shortestdistances; //shortest distances to each room from the start

            }

            private static int MinimumDistance(int[] shortestdistances, bool[] possiblequickestroute)
            {
                int smallest = int.MaxValue; //ironic again
                int windex = -1; //like like winning index 
                for (int i = 0; i < shortestdistances.Length; i++)
                {
                    if (!possiblequickestroute[i] && shortestdistances[i] <= smallest)
                    {
                        smallest = shortestdistances[i];
                        windex = i;
                    }
                }
                return windex; //the next smallest distance
            }
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textyBox = sender as TextBox;
            List<string> roomnames = school.roomnamesgrab();
            //MessageBox.Show(Convert.ToString(roomnames.Count));
            bool val = false;
            if (textyBox.Text != "")
            {
                foreach (string roomname in roomnames)
                {
                    if (textyBox.Text.ToUpper() == roomname.ToUpper())
                    {
                        val = true;
                    }
                }
                if (!val)
                {
                    MessageBox.Show("Invalid room!!!");
                    textyBox.Text = "";
                }
            }
        }
        public void GoButton_Clicked(object sender, EventArgs e)
        {
            Label DirectiosnOutput = Controls["DirectiosnOutput"] as Label;
            TextBox InitialRoomInput = Controls["InitialRoomInput"] as TextBox;
            TextBox EndRoomInput = Controls["EndRoomInput"] as TextBox;
            List<string> roomnames = school.roomnamesgrab();
            if (InitialRoomInput.Text != "" && EndRoomInput.Text != "")
            {
                string usersroom = InitialRoomInput.Text;
                string endroom = EndRoomInput.Text;
                int startingroom = roomnames.IndexOf(usersroom);
                int finalroom = roomnames.IndexOf(endroom);
                int[] shortestdistances = ActualPathFindingAlgorithm.dijkstra(school, finalroom, startingroom, roomnames);
                double timedistance = (shortestdistances[roomnames.IndexOf(usersroom)] / 1.42);
                timedistance = Math.Round(timedistance, 1);
                DirectiosnOutput.Text = (usersroom + " → " + endroom + " | " + shortestdistances[roomnames.IndexOf(usersroom)] + "m | " + timedistance + "s");
                DirectiosnOutput.Text += "\r\n" + pathways;
            }
        }
        public void MapButton_Clicked(object sender, EventArgs e)
        {
            Form ImageForm = new ImageForm();
            ImageForm.Show();
        }

    }
}
