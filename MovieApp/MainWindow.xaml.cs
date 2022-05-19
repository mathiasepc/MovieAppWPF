using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace MovieApp
{
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            //opdatere min data.
            BindDataGrid();
        }
        #endregion

        #region Methods
        /// <summary>
        /// opretter en string med min conneciton i.
        /// </summary>
        public string ConnectionString
        {
            get => "Data Source=192.168.192.129;Initial Catalog=Movies;User ID=Mathias;Password=Kings600;Connect Timeout=30;Encrypt=False;" +
                "TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        /// <summary>
        /// laver en bind metode til databasen for, at refreshe eller tilføje databasen.
        /// </summary>
        private void BindDataGrid()
        {
            //laver en vej til min server bruger using for at den selv lukker.
            using SqlConnection sqlConnection = new(ConnectionString);

            //åbner vejen
            sqlConnection.Open();

            try
            {
                //instansiere en klasse SqlCommand
                string sqlCommand = new("SELECT * FROM Movies");

                //indsætter min data i sqlDataAdapter som jeg instansiere
                SqlDataAdapter sqlDataAdapter = new(sqlCommand, sqlConnection);

                //opretter fiktiv datatable af datagrid
                DataTable movies = new();

                //refresher eller add til dataTable
                sqlDataAdapter.Fill(movies);

                //tilføjer til min Tabel
                MovieTable.ItemsSource = movies.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// gør klar til at oprette. Ved at validere felterne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Valider_Opret(object sender, RoutedEventArgs e)
        {
            //opretter mine checker 
            bool checkerRating = Validation.CheckRatingTextBox(TextBoxRating.Text);
            bool checkerYear = Validation.CheckYearTextBox(TextBoxYear.Text);
            bool checkerTitle = Validation.CheackTitleTextBox(TextBoxTitle.Text);

            //kalder mine checker for felterne Rating og Year
            if (!checkerRating)
                MessageBox.Show("Det max 2 tal fra 0-10. Ingen kommatal.");
            if (!checkerYear)
                MessageBox.Show("Det max 4 tal og mellem 1878 og 2022");
            if (!checkerTitle)
                MessageBox.Show("Der skal være en titel på filmen.");
            if (checkerRating && checkerTitle && checkerYear)
                Opret();

        }

        /// <summary>
        /// opretter min film
        /// </summary>
        private void Opret()
        {
            //laver en vej til min server bruger using for at den selv lukker.
            using SqlConnection sqlConnection = new(ConnectionString);

            //åbner vejen
            sqlConnection.Open();

            //istansiere SqlCommand klassen og indsætter i databasen
            SqlCommand sqlCommand = new($"INSERT INTO Movies (Title, Year, Rating) values('{TextBoxTitle.Text}', '{TextBoxYear.Text}', '{TextBoxRating.Text}')");

            //tilføjer min ConnectionString til sqlCommand object
            sqlCommand.Connection = sqlConnection;

            //laver en show til brugerne så man ved den er tilføjet
            MessageBox.Show("Filmen er tilføjet.");

            //sender til min database
            sqlCommand.ExecuteNonQuery();

            //refresher min side
            BindDataGrid();
        }

        /// <summary>
        /// laver min slette funktion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //laver et forloo for at gå ind i rækken
            for (int i = 0; i < MovieTable.Items.Count; i++)
            {
                //når den finder de valgte items
                if (MovieTable.SelectedItems.Contains(MovieTable.Items[i]))
                {
                    //opretter en vej til min database
                    using SqlConnection sqlConnection = new(ConnectionString);

                    //åbner vejen
                    sqlConnection.Open();

                    try
                    {
                        //finder rækken
                        DataRowView row = (DataRowView)MovieTable.SelectedItems[0];

                        //tager index 0 fra række og putter i display
                        int display = (int)row.Row.ItemArray[0];

                        //istansiere klassen SqlCommand
                        string sqlCommand = $"Delete from Movies Where ID ='{display}'";

                        //laver en adapter til sql sådan så den kan opdaterer min tabel
                        SqlDataAdapter sqlDataAdapter = new();

                        //putter min sql commando og connectionstring i deleteCommand
                        sqlDataAdapter.DeleteCommand = new(sqlCommand, sqlConnection);

                        //verificere for brugeren den er fjernet
                        MessageBox.Show("Filmen er fjernet.");

                        //eksekverer commandoen
                        sqlDataAdapter.DeleteCommand.ExecuteNonQuery();

                        //sletter rækken
                        sqlDataAdapter.Dispose();

                        //refresher min side
                        BindDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// gør klar til edit ved at validere felterne, clear felterne og erstatte button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            //skifter knap
            OpretButton.Visibility = Visibility.Collapsed;
            ConfirmButton.Visibility = Visibility.Visible;

            //laver en clear på felterne så man kan skrive nyt
            TextBoxTitle.Clear();
            TextBoxYear.Clear();
            TextBoxRating.Clear();
        }

        /// <summary>
        /// gør klar til update ved at validere felterne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Valider_Confirm(object sender, RoutedEventArgs e)
        {
            //opretter mine checker 
            bool checkerRating = Validation.CheckRatingTextBox(TextBoxRating.Text);
            bool checkerYear = Validation.CheckYearTextBox(TextBoxYear.Text);
            bool checkerTitle = Validation.CheackTitleTextBox(TextBoxTitle.Text);

            //kalder mine checker for felterne Rating og Year
            if (!checkerRating)
                MessageBox.Show("Det max 2 tal fra 0-10. Ingen kommatal.");
            if (!checkerYear)
                MessageBox.Show("Det max 4 tal og mellem 1878 og 2022");
            if (!checkerTitle)
                MessageBox.Show("Der skal være en titel på filmen.");
            if (checkerRating && checkerTitle && checkerYear)
            {
                Confirm();
            }

            //ændre knap efter ændringerne
            ConfirmButton.Visibility = Visibility.Hidden;
            OpretButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// laver update metoden
        /// </summary>
        private void Confirm()
        {
            //laver et forloo for at gå ind i rækken
            for (int i = 0; i < MovieTable.Items.Count; i++)
            {
                //når den finder de valgte items
                if (MovieTable.SelectedItems.Contains(MovieTable.Items[i]))
                {
                    //opretter en vej til min database
                    using SqlConnection sqlConnection = new(ConnectionString);

                    //åbner vejen
                    sqlConnection.Open();

                    try
                    {
                        DataRowView row = (DataRowView)MovieTable.SelectedItems[0];

                        //tager index 0 fra række og putter i display
                        int display = (int)row.Row.ItemArray[0];

                        //istansiere klassen SqlCommand
                        string change = $"UPDATE Movies SET Title = '{TextBoxTitle.Text}', Year = '{TextBoxYear.Text}', Rating = {TextBoxRating.Text} Where ID = {display}";

                        //laver en adapter til sql sådan så den kan opdaterer min tabel
                        SqlDataAdapter sqlDataAdapter = new();

                        //putter min sql commando og connectionstring i deleteCommand
                        sqlDataAdapter.DeleteCommand = new(change, sqlConnection);

                        //verificere for brugeren den er fjernet
                        MessageBox.Show("Filmen er opdateret.");

                        //eksekverer commandoen
                        sqlDataAdapter.DeleteCommand.ExecuteNonQuery();

                        //sletter rækken
                        sqlDataAdapter.Dispose();

                        //refresher min side
                        BindDataGrid();

                        //ændre knap efter ændringerne
                        ConfirmButton.Visibility = Visibility.Hidden;
                        OpretButton.Visibility = Visibility.Visible;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        #endregion
    }
}
