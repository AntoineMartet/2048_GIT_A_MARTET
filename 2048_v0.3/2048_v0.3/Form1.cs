// Fichier : Form1.cs
// Projet : 2048_v0.3
// Auteur : Antoine MARTET
// Création : mercredi 02.12.2022
// Dernière MAJ : lundi 14.01.2023 par Antoine MARTET
// But du programme (sprint 4):
//    - Finaliser le développement de l'application 2048 et y implémenter des options supplémentaires.
// Options choisies :
//    - Affichage du score de la partie en cours et du record de score (sauvegardé automatiuement dans un fichier)
//    - Affichage du chrono de la partie en cours et du record de temps (sauvegardé automatiquement dans un fichier)
//    - Bouton de pause interactif (Raccourci : P)
//    - Fonction pour annuler le dernier mouvement (Raccourci : Espace)
//    - Menus pour commencer une nouvelle partie, pour mettre en pause la partie, pour annuler le dernier déplacement, pour quitter l'application
//    - Image en background
//    - Fenêtre non redimensionnable pour garder une apparence et des proportions cohérentes
//    - Messages des différentes fins de parties affinés (fin de partie sans victoire au préalable, fin de partie avec victoire au préalable, mention du score et du chrono)
//    - Menu Développeur toujours présent dans le code mais en Visible = false

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace _2048_v0._3
{
    // Initialise une nouvel objet (nommée Form1) de la classe Form.
    // Initialiser un objet = instancier
    public partial class Form1 : Form
    {
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Variables globales et initialisation du formulaire

        Label label_background = new Label();
        Label label_record_text = new Label();
        Label label_record_number = new Label();
        Label label_score_text = new Label();
        Label label_score_number = new Label();
        Label label_time = new Label();
        Label label_time_record = new Label();

        Label[,] array_labels = new Label[4, 4];     // Tableau de 4x4 labels (pour affichage)

        Label label_victory_background = new Label();
        Label label_victory_message = new Label();
        Label label_victory_continue = new Label();
        Label label_victory_restart = new Label();

        Label label_fail_background = new Label();
        Label label_fail_message = new Label();
        Label label_fail_continue = new Label();
        Label label_fail_restart = new Label();

        Label label_pause = new Label();

        Font MediumFont = new Font("Microsoft Sans Serif", 10); // Police de base (score, record de score, temps, record de temps)

        Random random = new Random();
        bool homePage = true;
        bool playing = true;
        bool hasWon = false;
        int seconds = 0;
        int score = 0;
        int scoreRecord;
        TimeSpan time = new TimeSpan();
        TimeSpan timeRecord = new TimeSpan();

        int[,] array_example_beginning = { { 2, 0, 0, 2 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };  // Tableau de 4x4 entiers (pour démo début de partie)
        int[,] array_example_ingame = { { 128, 32, 32, 0 }, { 4, 8, 4, 8 }, { 8, 16, 4, 512 }, { 32, 256, 8, 8 } };  // Tableau de 4x4 entiers (pour démo milieu de partie)
        int[,] array_example_test = { { 4, 2, 1024, 1024 }, { 32, 4, 2, 128 }, { 2, 16, 4, 512 }, { 16, 32, 64, 128 } };  // Tableau de 4x4 entiers (pour démo fin de partie)
        int[,] array_memory = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };  // Tableau principal
        int[,] array_cancel = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };  // Tableau pour sauvegarder temporairement l'état de array_memory pour annuler un déplacement si besoin
        Color[] Couleur = {Color.DimGray,                   Color.FromArgb(0, 0, 176),      Color.FromArgb(31, 19, 245),    Color.FromArgb(85, 85, 255),
                           Color.FromArgb(133, 133, 255),   Color.FromArgb(200, 200, 255),  Color.FromArgb(228, 228, 255),  Color.FromArgb(255, 255, 255),
                           Color.FromArgb(240, 207, 207),   Color.FromArgb(234, 153, 153),  Color.FromArgb(224, 95, 95),    Color.FromArgb(207, 42, 39),
                           Color.FromArgb(170, 0, 0),       Color.FromArgb(119, 0, 0)};

        public Form1()
        {
            // Crée l'interface utilisateur réalisée "à la main" sur l'onglet Form1.cs [Design]
            InitializeComponent();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Fonctions perso

        // Met le timer à 0 et l'active
        private void fnTimerToZero()
        {
            // "seconds = 0" ne suffit pas car le timer est toujours activé et donc la 1ère "nouvelle" seconde durera entre 0 et 1000 ms au lieu de 1000 ms.
            // D'où l'enchaînement de la désactivation et de la réactivation du timer. Le "timer_game.Enabled = false" remet le timer à 0.
            // "Temps : 00:00:00" sert à afficher le temps à 0 immédiatement plutôt que d'attendre le prochain tick du timer.

            seconds = 0;
            label_time.Text = "Temps : 00:00:00";
            timer_game.Enabled = false;
            timer_game.Enabled = true;
        }

        // Affiche une grille préexistante ou crée une grille aléatoire de début de jeu
        private void fnStartNewGrid(int[,] array)
        {
            fnVictoryLabelsNotVisible();
            fnFailLabelsNotVisible();
            homePage = false;
            hasWon = false;
            playing = true;
            score = 0;
            label_score_number.Text = "0";
            fnTimerToZero();
            picture_play_pause.Visible = true;
            picture_play_pause.Image = global::_2048_v0._3.Properties.Resources.pause_button;
            label_pause.Visible = false;

            if (array == array_memory)
            {
                fn2DArrayToZero(array_memory);
                List<int> list_zero = fnListCoordZero();
                fnCreateTile(list_zero);
                list_zero = fnListCoordZero();
                fnCreateTile(list_zero);
            }
            else
            {
                fnCopy2DArray(array, array_memory);
            }

            fnDisplay(array_memory);
            fnCopy2DArray(array_memory, array_cancel);

            /*
            
            /!\ NE SURTOUT PAS FAIRE CA :
            
            array_memory = array_example_ingame;
            fnDisplay(array_memory);

            /!\ In C#, arrays are of reference type and not of value type. The value of array_example_ingame is just a reference to a byte array.
            So after that assignment, changed made to the byte array "via" one variable will be visible "via" the other variable, as they now both
            reference to the same byte array (they point to the same memory location).
            Pour des array 1D on peut utiliser la méthode Array.CopyTo : sourceArray.CopyTo(destinationArray, startingIndex);
            Pour des array 2D le mieux pour l'instant est de faire une double boucle for.

            Sources :
            https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types?redirectedfrom=MSDN
            https://stackoverflow.com/questions/29398848/c-assign-array-to-another-array-copy-or-pointer-exchange
            https://learn.microsoft.com/en-us/dotnet/api/system.array.copyto?redirectedfrom=MSDN&view=net-7.0#System_Array_CopyTo_System_Array_System_Int32_

            */
        }

        // Déclaration de la fonction appelée pour :
        // 1) copier dans les labels (en string) les valeurs des tableaux d'entiers
        // 2) afficher les tuiles > à 0 et masquer les tuiles = à 0
        // 3) associer la couleur prédéfinie pour la valeur de la tuile
        private void fnDisplay(int[,] array)
        {
            for (int i = 0; i < 4; i++)              // Lignes
            {
                for (int j = 0; j < 4; j++)          // Colonnes
                {
                    array_labels[i, j].Text = array[i, j].ToString();
                    if (array[i, j] > 0)
                    {
                        // Pour Couleur[n] : n = log en base 2 de la valeur de array[i, j]
                        array_labels[i, j].BackColor = Couleur[(int)(Math.Log((double)array[i, j], 2))];
                        if (array[i, j] >= 16 && array[i, j] <= 512)
                        {
                            array_labels[i, j].ForeColor = Color.Black;
                        }
                        else
                        {
                            array_labels[i, j].ForeColor = Color.White;
                        }
                    }
                    else
                    {
                        array_labels[i, j].BackColor = Couleur[0];
                        array_labels[i, j].Text = "";
                    }
                }
            }
        }

        // Fonction de tassement à 4 paramètres
        // Envoyer les arguments [i,0][i,1][i,2][i,3] pour tasser à gauche (i est la ligne)
        // Envoyer les arguments [i,3][i,2][i,1][i,0] pour tasser à droite (i est la ligne)
        // Envoyer les arguments [0,j][1,j][2,j][3,j] pour tasser en haut (j est la colonne)
        // Envoyer les arguments [3,j][2,j][1,j][0,j] pour tasser en bas (j est la colonne)
        private int[] fnMerge(int a, int b, int c, int d, out int i)
        {
            i = 0;  // Compteur de changement. Vaut 1 ou 0 à la fin de la fonction selon qu'il y a eu au moins un changement ou non dans la ligne à tasser.
            int[] array_temp = { a, b, c, d };  // Sauvegarde les paramètres reçus pour les comparer avec les valeurs qui seront renvoyées (array_result)
            int[] array_result = new int[4];

            // Supprimer tous les 0 dans la ligne :
            if (c == 0)
            {
                c = d;
                d = 0;
            }
            if (b == 0)
            {
                b = c;
                c = d;
                d = 0;
            }
            if (a == 0)
            {
                a = b;
                b = c;
                c = d;
                d = 0;
            }

            // Fusionner les paires et décaler à gauche ce qu’il y avait à droite de la ou des paire(s) :
            if (a == b && a != 0)
            {
                a *= 2;
                b = c;
                c = d;
                d = 0;
                score += a;
            }
            if (b == c && b != 0)
            {
                b *= 2;
                c = d;
                d = 0;
                score += b;
            }
            if (c == d && c != 0)
            {
                c *= 2;
                d = 0;
                score += c;
            }

            array_result[0] = a;
            array_result[1] = b;
            array_result[2] = c;
            array_result[3] = d;

            // Concaténation du contenu des cellules
            string temp = String.Join("", array_temp);
            string line = String.Join("", array_result);

            // Si la ligne pré-tassement est différente de la ligne post-tassement, incrémenter la variable changes
            if (temp != line)
            {
                i++;
            }

            return array_result;
        }

        // Affecte les valeurs d'un tableau 2D source à un tableau 2D de destination
        private void fnCopy2DArray(int[,] array_source, int[,] array_destination)
        {
            for (int i = 0; i < array_source.GetLength(0); i++)
            {
                for (int j = 0; j < array_source.GetLength(1); j++)
                {
                    array_destination[i, j] = array_source[i, j];
                }
            }
        }

        // Active ou désactive les éléments du test de la fonction de tassement
        private void fnDisplayTest(bool state)
        {
            txtTuile1.Visible = state;
            txtTuile2.Visible = state;
            txtTuile3.Visible = state;
            txtTuile4.Visible = state;
            btTasser.Visible = state;
            txtResult.Visible = state;
            label_changements.Visible = state;
        }

        // Affectation des résultats du tassement au tableau mémoire du jeu
        // l est un booléen qui détermine si l'affectation concerne une ligne (true) ou une colonne (false)
        // i représente l'index de la ligne ou de la colonne concernée
        private void fnAssignAfterMerge(bool l, int i, int a, int b, int c, int d)
        {
            if (l == true)
            {
                array_memory[i, 0] = a;
                array_memory[i, 1] = b;
                array_memory[i, 2] = c;
                array_memory[i, 3] = d;
            }
            else
            {
                array_memory[0, i] = a;
                array_memory[1, i] = b;
                array_memory[2, i] = c;
                array_memory[3, i] = d;
            }
        }

        // Met à 0 toutes les cellules d'un tableau 2D
        private void fn2DArrayToZero(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    array[i, j] = 0;
                }
            }
        }

        // Crée une liste des emplacements (de 0 à 15) des 0 dans array_memory
        private List<int> fnListCoordZero()
        {
            List<int> listeCoordZero = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (array_memory[i, j] == 0)
                    {
                        listeCoordZero.Add(4 * i + j % 4);
                    }
                }
            }

            return listeCoordZero;
        }

        // Crée une tuile de valeur 2 ou 4 si un changement a eu lieu suite à une tentative de tassement
        private void fnCreateTile(List<int> liste)
        {

            int index_random = random.Next(liste.Count);

            // Maintenant on veut mettre un 2 dans la tuile de memory_array qui correspond à la valeur stockée à l'index index_random de array
            int position = liste[index_random];
            int ligne = position / 4;
            int col = position % 4;

            // Déterminer au hasard si on crée un 2 (7 chances sur 8) ou un 4 (1 chance sur 8)
            int resultRandom = random.Next(9);
            if (resultRandom == 0)
            {
                array_memory[ligne, col] = 4;
            }
            else
            {
                array_memory[ligne, col] = 2;
            }
        }

        // Regarde s'il y a un 2048 dans la grille. Si oui, renvoie true; si non, renvoie false.
        // Cette fonction est appelée seulement s'il y a eu un changement et si la partie n'a pas encore été gagnée (hasWon == false).
        private bool fnCheckVictory()
        {
            // Vérifier s'il y a un 2048 dans la grille
            for (int i = 0; i < 4; i++)              // Lignes
            {
                for (int j = 0; j < 4; j++)          // Colonnes
                {
                    if (array_memory[i, j] == 2048)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Regarde s'il y a des tuiles adjacentes de même valeur dans la grille. Si oui (= pas de défaite), retourne false. Si non (= défaite), retourne true.
        private bool fnCheckFail()
        {
            bool hasFailed = true;

            // Recherche de paires avec lecture horizontale de la grille, ligne par ligne
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (array_memory[i, j] == array_memory[i, j + 1])
                    {
                        hasFailed = false;
                    }
                }
            }

            // Recherche de paires avec lecture verticale de la grille, colonne par colonne
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (array_memory[i, j] == array_memory[i + 1, j])
                    {
                        hasFailed = false;
                    }
                }
            }

            // Si on a trouvé aucune paire, alors on a perdu (car fnCheckFail() n'est appelée que lorsqu'il n'y a déjà plus de cases vides)
            if (hasFailed == true)
            {
                return true;
            }else
            {
                return false;
            }
        }

        // Crée et affiche les labels de victoire et les évènements liés (Continuer la partie, Commencer une nouvelle partie)
        private void fnDisplayVictoryLabels()
        {
            picture_play_pause.Visible = false;

            label_victory_background.Bounds = new Rectangle(170, 170, 410, 410);
            label_victory_background.BackColor = Color.FromArgb(255, 128, 0);
            label_victory_background.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(label_victory_background);
            label_victory_background.Visible = true;
            label_victory_background.BringToFront();

            label_victory_message.Bounds = new Rectangle(175, 170 + 30, 400, 300);
            label_victory_message.Font = new Font("Arial", 18);
            label_victory_message.Text = "Félicitation !\nVous avez gagné !\n\nScore : " + score + "\nDurée : " + time.ToString(@"hh\:mm\:ss") + "\n\nVoulez-vous continuer la partie ou en commencer une nouvelle ?\n\n";
            label_victory_message.TextAlign = ContentAlignment.MiddleCenter;
            label_victory_message.BackColor = Color.FromArgb(255, 128, 0);
            Controls.Add(label_victory_message);
            label_victory_message.BringToFront();
            label_victory_message.Visible = true;

            label_victory_continue.Bounds = new Rectangle(210, 475, 150, 40);
            label_victory_continue.Font = new Font("Arial", 15);
            label_victory_continue.Text = "Continuer";
            label_victory_continue.TextAlign = ContentAlignment.MiddleCenter;
            label_victory_continue.BackColor = Color.FromArgb(255, 255, 0);
            label_victory_continue.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(label_victory_continue);
            label_victory_continue.BringToFront();
            label_victory_continue.Visible = true;
            label_victory_continue.Click += new EventHandler(label_victory_continue_Click);

            label_victory_restart.Bounds = new Rectangle(390, 475, 150, 40);
            label_victory_restart.Font = new Font("Arial", 15);
            label_victory_restart.Text = "Nouvelle partie";
            label_victory_restart.TextAlign = ContentAlignment.MiddleCenter;
            label_victory_restart.BackColor = Color.FromArgb(255, 255, 0);
            label_victory_restart.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(label_victory_restart);
            label_victory_restart.BringToFront();
            label_victory_restart.Visible = true;
            label_victory_restart.Click += new EventHandler(label_victory_restart_Click);
        }

        // Crée et affiche les labels de défaite et les évènements liés (Voir la grille, Commencer une nouvelle partie)
        private void fnDisplayFailLabels()
        {
            picture_play_pause.Visible = false;

            label_fail_background.Bounds = new Rectangle(170, 170, 410, 410);
            label_fail_background.BackColor = Color.FromArgb(255, 128, 0);
            label_fail_background.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(label_fail_background);
            label_fail_background.Visible = true;
            label_fail_background.BringToFront();

            label_fail_message.Bounds = new Rectangle(175, 170 + 30, 400, 300);
            label_fail_message.Font = new Font("Arial", 18);
            if(hasWon == false)
            {
                label_fail_message.Text = "Dommage !\nVous avez perdu !\n\nScore : " + score + "\nDurée : " + time.ToString(@"hh\:mm\:ss") + "\n\nVoulez-vous voir la grille ou commencer une nouvelle partie ?\n\n";
            }
            else
            {
                label_fail_message.Text = "Dommage !\nCette fois-ci c'est fini !\n\nScore : " + score + "\nDurée : " + time.ToString(@"hh\:mm\:ss") + "\n\nVoulez-vous voir la grille ou commencer une nouvelle partie ?\n\n";
            }
            label_fail_message.TextAlign = ContentAlignment.MiddleCenter;
            label_fail_message.BackColor = Color.FromArgb(255, 128, 0);
            Controls.Add(label_fail_message);
            label_fail_message.BringToFront();
            label_fail_message.Visible = true;

            label_fail_continue.Bounds = new Rectangle(210, 475, 150, 40);
            label_fail_continue.Font = new Font("Arial", 15);
            label_fail_continue.Text = "Voir la grille";
            label_fail_continue.TextAlign = ContentAlignment.MiddleCenter;
            label_fail_continue.BackColor = Color.FromArgb(255, 255, 0);
            label_fail_continue.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(label_fail_continue);
            label_fail_continue.BringToFront();
            label_fail_continue.Visible = true;
            label_fail_continue.Click += new EventHandler(label_fail_continue_Click);

            label_fail_restart.Bounds = new Rectangle(390, 475, 150, 40);
            label_fail_restart.Font = new Font("Arial", 15);
            label_fail_restart.Text = "Nouvelle partie";
            label_fail_restart.TextAlign = ContentAlignment.MiddleCenter;
            label_fail_restart.BackColor = Color.FromArgb(255, 255, 0);
            label_fail_restart.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(label_fail_restart);
            label_fail_restart.BringToFront();
            label_fail_restart.Visible = true;
            label_fail_restart.Click += new EventHandler(label_fail_restart_Click);
        }

        // Rend invisibles les labels de victoire
        private void fnVictoryLabelsNotVisible()
        {
            label_victory_background.Visible = false;
            label_victory_message.Visible = false;
            label_victory_continue.Visible = false;
            label_victory_restart.Visible = false;
        }

        // Rend invisibles les labels de défaite
        private void fnFailLabelsNotVisible()
        {
            label_fail_background.Visible = false;
            label_fail_message.Visible = false;
            label_fail_continue.Visible = false;
            label_fail_restart.Visible = false;
        }

        // Affiche le message de pause et cache la grille
        private void fnDisplayPause()
        {
            label_pause.Bounds = new Rectangle(170, 170, 410, 410);
            label_pause.BackColor = Color.FromArgb(255, 128, 0);
            label_pause.Font = new Font("Arial", 18);
            label_pause.Text = "Partie en pause";
            label_pause.TextAlign = ContentAlignment.MiddleCenter;
            label_pause.BorderStyle = BorderStyle.FixedSingle;
            label_pause.TabIndex = 8;
            Controls.Add(label_pause);
            label_pause.Visible = true;
            label_pause.BringToFront();
        }

        // Mise à jour de la variable scoreRecord, de son affichage, et du fichier qui contient le record de score
        private void fnUpdateScoreRecord()
        {
            scoreRecord = score;
            label_record_number.Text = Convert.ToString(scoreRecord);
            using (StreamWriter writer = new StreamWriter(@"ScoreRecord.txt", false))
            {
                writer.Write(scoreRecord);
            }
        }

        // Mise à jour de la variable timeRecord, de son affichage, et du fichier qui contient le record de temps
        private void fnUpdateTimeRecord()
        {
            timeRecord = time;
            label_time_record.Text = "Record : " + timeRecord.ToString(@"hh\:mm\:ss");
            using (StreamWriter writer = new StreamWriter(@"TimeRecord.txt"))
            {
                writer.Write(time.Seconds);
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Fonctions d'évènements

        // Crée les différents labels au lancement du jeu (grille, score, record...)
        private void Form1_Load(object sender, EventArgs e)     // Fonction appelée au lancement du programme (à l'ouverture de la fenêtre)
        {
            //On crée les labels dans le form load (texte vide)
            for (int i = 0; i < 4; i++)              // Lignes
            {
                for (int j = 0; j < 4; j++)          // Colonnes
                {
                    // Création du label et de ses propriétés, mais pas encore d'affichage
                    array_labels[i, j] = new Label();
                    array_labels[i, j].Bounds = new Rectangle(180 + 100 * j, 180 + 100 * i, 90, 90);
                    array_labels[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    array_labels[i, j].Font = new Font("Arial", 20);
                    array_labels[i, j].BackColor = Color.DimGray;
                    array_labels[i, j].Text = "";
                    // Affichage du label créé
                    Controls.Add(array_labels[i, j]);
                }
            }
            // On créer un label pour le fond de la grille
            label_background.Bounds = new Rectangle(170, 170, 410, 410);
            label_background.BackColor = Color.Gray;
            Controls.Add(label_background);
            label_background.SendToBack();  // L'inverse : BringToFront()

            // On crée un label pour le texte "Score : " aligné à gauche
            label_score_text.Bounds = new Rectangle(450, 145, 130, 20);
            label_score_text.BackColor = Color.Gray;
            label_score_text.ForeColor = Color.White;
            label_score_text.Text = "Score : ";
            label_score_text.TextAlign = ContentAlignment.MiddleLeft;
            label_score_text.Font = MediumFont;
            Controls.Add(label_score_text);

            // On crée un label pour le nombre du score aligné à droite
            label_score_number.Bounds = new Rectangle(510, 145, 70, 20);
            label_score_number.BackColor = Color.Gray;
            label_score_number.ForeColor = Color.White;
            label_score_number.Text = "0";
            label_score_number.TextAlign = ContentAlignment.MiddleRight;
            label_score_number.Font = MediumFont;
            Controls.Add(label_score_number);
            label_score_number.BringToFront();

            // On crée un label pour le texte "Record : " aligné à gauche
            label_record_text.Bounds = new Rectangle(450, 120, 130, 20);
            label_record_text.BackColor = Color.Gray;
            label_record_text.ForeColor = Color.White;
            label_record_text.Text = "Record : ";
            label_record_text.TextAlign = ContentAlignment.MiddleLeft;
            label_record_text.Font = MediumFont;
            Controls.Add(label_record_text);

            // On obtient le record de score stocké dans un fichier
            using (StreamReader reader = new StreamReader(@"ScoreRecord.txt"))
            {
                scoreRecord = Convert.ToInt32(reader.ReadLine());
            }

            // On crée un label pour le nombre du record de score aligné à droite
            label_record_number.Bounds = new Rectangle(510, 120, 70, 20);
            label_record_number.BackColor = Color.Gray;
            label_record_number.ForeColor = Color.White;
            label_record_number.Text = Convert.ToString(scoreRecord);
            label_record_number.TextAlign = ContentAlignment.MiddleRight;
            label_record_number.Font = MediumFont;
            Controls.Add(label_record_number);
            label_record_number.BringToFront();

            // On crée un label pour le temps
            label_time.Bounds = new Rectangle(170, 145, 130, 20);
            label_time.BackColor = Color.Gray;
            label_time.ForeColor = Color.White;
            label_time.Text = "Temps : 00:00:00";
            label_time.TextAlign = ContentAlignment.MiddleLeft;
            label_time.Font = MediumFont;
            Controls.Add(label_time);

            // On obtient le record de temps stocké dans un fichier
            using (StreamReader reader = new StreamReader(@"TimeRecord.txt"))
            {
                timeRecord = TimeSpan.FromSeconds(Convert.ToInt32(reader.ReadLine()));
            }

            // On crée un label pour le record de temps
            label_time_record.Bounds = new Rectangle(170, 120, 130, 20);
            label_time_record.BackColor = Color.Gray;
            label_time_record.ForeColor = Color.White;
            label_time_record.Text = "Record : " + timeRecord.ToString(@"hh\:mm\:ss");
            label_time_record.TextAlign = ContentAlignment.MiddleLeft;
            label_time_record.Font = MediumFont;
            Controls.Add(label_time_record);

            // On donne au background du bouton de pause la même couleur que l'image derrière

        }

        // Affiche le tableau mémoire de début de partie
        private void displayBeginningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fnStartNewGrid(array_example_beginning);
        }

        // Affiche le tableau mémoire de milieu de partie avec tous les nombres possibles
        private void displayExampleInGameGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fnStartNewGrid(array_example_ingame);
        }

        // Affiche le tableau de mémoire de test
        private void displayExampleTestGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fnStartNewGrid(array_example_test);
        }

        // Créer une grille aléatoire de début de jeu
        private void startNewGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fnStartNewGrid(array_memory);
        }

        // Activer ou désactiver le test du tassement
        private void enableTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtTuile1.Visible)
            {
                fnDisplayTest(false);
            }
            else
            {
                fnDisplayTest(true);
            }
        }

        // Tassement pour le test
        private void btTasser_Click(object sender, EventArgs e)
        {
            int changes_temp;
            int[] array_test_after = fnMerge(Int32.Parse(txtTuile1.Text), Int32.Parse(txtTuile2.Text), Int32.Parse(txtTuile3.Text), Int32.Parse(txtTuile4.Text), out changes_temp);
            txtResult.Text = array_test_after[0].ToString() + " " + array_test_after[1].ToString() + " " + array_test_after[2].ToString() + " " + array_test_after[3].ToString();

            if (changes_temp > 0)
            {
                label_changements.Text = "Changements : oui";
            }
            else
            {
                label_changements.Text = "Changements : non";
            }
        }

        // Essaie de tasser, crée une tuile si nécessaire, checke la victoire et la défaite
        // S'exécute quand on appuie sur n'importe quelle touche du clavier
        // e contient des infos sur l'évènement, dont le KeyCode (la touche appuyée)
        // Si on crée un bouton, écrire Form1_KeyDown(sender, e) dans Bouton1_Keydown pour transmettre l'info des touches appuyées à Form1
        // Ca marchera pour ASDW mais pas pour les flèches
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            int changes = 0;
            int changes_temp;
            int[] array_line;
            bool ligne;
            int[,] array_cancel_temp = { { 2, 0, 0, 2 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            fnCopy2DArray(array_memory, array_cancel_temp);

            if (e.KeyCode == Keys.P)
            {
                picture_play_pause_Click(sender, e);
            }

            if (playing == true)
            {
                if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) // Tasser à gauche
                {
                    ligne = true;
                    for (int i = 0; i < 4; i++) // i = numéro de ligne
                    {
                        array_line = fnMerge(array_memory[i, 0], array_memory[i, 1], array_memory[i, 2], array_memory[i, 3], out changes_temp);
                        changes += changes_temp;

                        // Affectation des résultats du tassement (array_line) au tableau mémoire du jeu (array_memory)
                        fnAssignAfterMerge(ligne, i, array_line[0], array_line[1], array_line[2], array_line[3]);
                    }
                }
                else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) // Tasser à droite
                {
                    ligne = true;
                    for (int i = 0; i < 4; i++) // i = numéro de ligne
                    {
                        array_line = fnMerge(array_memory[i, 3], array_memory[i, 2], array_memory[i, 1], array_memory[i, 0], out changes_temp);
                        changes += changes_temp;

                        fnAssignAfterMerge(ligne, i, array_line[3], array_line[2], array_line[1], array_line[0]);
                    }
                }
                else if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up) // Tasser en haut
                {
                    ligne = false;
                    for (int i = 0; i < 4; i++) // i = numéro de colonne
                    {
                        array_line = fnMerge(array_memory[0, i], array_memory[1, i], array_memory[2, i], array_memory[3, i], out changes_temp);
                        changes += changes_temp;

                        fnAssignAfterMerge(ligne, i, array_line[0], array_line[1], array_line[2], array_line[3]);
                    }
                }
                else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down) // Tasser en bas
                {
                    ligne = false;
                    for (int i = 0; i < 4; i++) // i = numéro de colonne
                    {
                        array_line = fnMerge(array_memory[3, i], array_memory[2, i], array_memory[1, i], array_memory[0, i], out changes_temp);
                        changes += changes_temp;

                        fnAssignAfterMerge(ligne, i, array_line[3], array_line[2], array_line[1], array_line[0]);
                    }
                }
                else if (e.KeyCode == Keys.Back)
                {
                    fnCopy2DArray(array_cancel, array_memory);
                    fnDisplay(array_memory);
                }

                 List<int> list_zero = fnListCoordZero();

                // Si changement : on crée une nouvelle tuile; on actualise le score et array_cancel; on checke le record, la victoire et la défaite
                if (changes > 0)
                {
                    fnCopy2DArray(array_cancel_temp, array_cancel);
                    label_changements.Text = "Changements : oui";
                    fnCreateTile(list_zero);
                    fnDisplay(array_memory);
                    list_zero = fnListCoordZero(); // MAJ de la liste pour la fonction fnCheckFail un peu plus bas
                    label_score_number.Text = Convert.ToString(score);

                    if (score > scoreRecord)
                    {
                        fnUpdateScoreRecord();
                    }

                    // Si changement ET aucune tuile vide : on checke la défaite
                    if (list_zero.Count == 0)
                    {
                        if (fnCheckFail()) // fnCheckFail() renvoie true si la partie est perdue et false dans le cas contraire
                        {
                            fnDisplayFailLabels(); 
                            playing = false;
                        }
                    }

                    // Si changement ET partie pas encore gagnée : on checke la victoire
                    if (hasWon == false)
                    {
                        if (fnCheckVictory())  // fnCheckVictory() renvoie true s'il y a eu victoire et false dans le cas contraire
                        {
                            hasWon = true;
                            playing = false;
                            fnDisplayVictoryLabels();

                            if (timeRecord.Seconds == 0) // Cette condition est utile pour l'inscription du 1er record car au départ le fichier de record contient "0".
                            {
                                fnUpdateTimeRecord();
                            }
                            else if (time < timeRecord)
                            {
                                fnUpdateTimeRecord();
                            }
                        }
                    }

                    /* NB : il est possible d'à la fois gagner et perdre. Du coup pour l'instant on checke la victoire en 2e pour que le message
                     * de félicitation apparaisse en premier. Celui de défaite est toujours affiché mais juste en dessous.
                     */
                }
                else
                {
                    label_changements.Text = "Changements : non";
                }
            }
        }

        // Rend invisibles les labels de victoire
        private void label_victory_continue_Click(object sender, EventArgs e)
        {
            fnVictoryLabelsNotVisible();

            // Si fnCheckFail renvoie false, alors les touches de jeu, le bouton de pause et le compteur (conditionné par playing = true) doivent redevenir actifs
            if (!fnCheckFail())
            {
                playing = true;
                picture_play_pause.Visible = true;
            }
        }

        // Rend invisibles les labels de victoire et commence une nouvelle partie
        private void label_victory_restart_Click(object sender, EventArgs e)
        {
            fnVictoryLabelsNotVisible();
            fnStartNewGrid(array_memory);
        }

        // Rend invisibles les labels de défaite
        private void label_fail_continue_Click(object sender, EventArgs e)
        {
            fnFailLabelsNotVisible();
        }

        // Rend invisibles les labels de défaite et commence une nouvelle partie
        private void label_fail_restart_Click(object sender, EventArgs e)
        {
            fnFailLabelsNotVisible();
            fnStartNewGrid(array_memory);
        }

        // Affiche le temps écoulé pour la partie en cours
        private void timer_game_Tick(object sender, EventArgs e)
        {
            if (playing == true)
            {
                seconds += 1;
                time = TimeSpan.FromSeconds(seconds);
                label_time.Text = "Temps : " + time.ToString(@"hh\:mm\:ss");
            }
        }

        // Change l'image du bouton play/pause et met en pause ou reprend la partie
        private void picture_play_pause_Click(object sender, EventArgs e)
        {
            if(!homePage)
            {
                if (playing)
                {
                    playing = false;
                    picture_play_pause.Image = global::_2048_v0._3.Properties.Resources.play_button;
                    fnDisplayPause();
                }
                else
                {
                    playing = true;
                    picture_play_pause.Image = global::_2048_v0._3.Properties.Resources.pause_button;
                    label_pause.Visible = false;
                }
            }
        }

        private void quitterLapplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}