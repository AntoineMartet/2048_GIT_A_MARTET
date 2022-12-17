// Fichier : Form1.cs
// Projet : 2048_v0.3
// Auteur : Antoine MARTET
// Création : mercredi 02.12.2022
// Dernière MAJ : lundi 13.12.2022 par Antoine MARTET
// But du programme (sprint 3) :
//    - Si un tassement a eu lieu, faire apparaître aléatoirement un 2 ou un 4 dans une case encore vide
//    - Déterminer après chaque tassement ou tentative de tassement si la partie est perdue ou gagnée et afficher un message le cas échéant

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048_v0._3
{
    // Initialise une nouvel objet (nommée Form1) de la classe Form.
    // Initialiser un objet = instancier
    public partial class Form1 : Form
    {
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Variables globales et initialisation du formulaire

        Label label_background = new Label();
        Label label_record = new Label();
        Label label_score = new Label();
        Label[,] array_labels = new Label[4, 4];     // Tableau de 4x4 labels (pour affichage)
        Random random = new Random();
        bool hasWon = false;
        bool hasLost = false;
        int[,] array_example_beginning = { { 2, 0, 0, 2 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };  // Tableau de 4x4 entiers (pour démo début de jeu)
        int[,] array_example_ingame = { { 4096, 8192, 1024, 0 }, { 2048, 4, 2, 128 }, { 8, 16, 4, 512 }, { 32, 256, 2048, 64 } };  // Tableau de 4x4 entiers (pour démo milieu de partie avec toutes les valeurs)
        int[,] array_example_test = { { 4096, 8192, 1024, 0 }, { 2048, 4, 2, 128 }, { 8, 16, 4, 512 }, { 2, 2, 2, 2 } };
        int[,] array_memory = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };  // Tableau de 4x4 entiers (pour démo milieu de jeu)
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

        // Créer et affiche une grille vide sauf 2 tuiles à 2 ou 4
        private void fnStartNewGame()
        {
            fn2DArrayToZero(array_memory);
            List<int> list_zero = fnListCoordZero();
            fnCreateTile(list_zero);
            list_zero = fnListCoordZero();
            fnCreateTile(list_zero);
            fnDisplay(array_memory);
            hasWon = false;
            hasLost = false;
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
                        if(array[i, j] >= 16 && array[i, j] <= 512)
                        {
                            array_labels[i, j].ForeColor = Color.Black;
                        }else
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
        // Envoyer les arguments [i,0][i,1][i,2][i,3] pour tasser à gauche
        // Envoyer les arguments [i,3][i,2][i,1][i,0] pour tasser à droite
        // Envoyer les arguments [0,i][1,i][2,i][3,i] pour tasser en haut
        // Envoyer les arguments [3,i][2,i][1,i][0,i] pour tasser en bas
        private int[] fnMerge(int a, int b, int c, int d, out int i)
        {
            i = 0;  // Compteur de changement. Vaut 1 ou 0 à la fin de la fonction selon qu'il y a eu au moins un changement ou non dans la ligne à tasser.
            int[] array_temp = { a, b, c, d };  // Sauvegarde les paramètres reçus (pour les comparer avec les valeurs qui seront renvoyées)
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
            }
            if (b == c && b != 0)
            {
                b *= 2;
                c = d;
                d = 0;
            }
            if (c == d && c != 0)
            {
                c *= 2;
                d = 0;
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
        private void fnActivationTest(bool state)
        {
            txtTuile1.Enabled = state;
            txtTuile2.Enabled = state;
            txtTuile3.Enabled = state;
            txtTuile4.Enabled = state;
            btTasser.Enabled = state;
            txtResult.Enabled = state;
        }

        // Affectation des résultats du tassement au tableau mémoire du jeu
        private void fnAffectationApresTassement(bool l, int i, int a, int b, int c, int d)
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
                        listeCoordZero.Add(4*i + j % 4);
                    }
                }
            }

            return listeCoordZero;

            /* Ancienne version avec array_zero :
            int zero_count = 0;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if (array_memory[i, j] == 0)
                    {
                        zero_count++;
                    }
                }
            }

            int[] array_zero = new int[zero_count];

            if (zero_count > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (array_memory[i, j] == 0)
                        {
                            // Transformer les i/j en numéro de case (1 à 16) ou utiliser un tableau 
                            // Alternative : créer un tableau (de longueur zero_count) de tableau de coordonnées (de longueur 2) : int[][] coord = ...

                            // On ajoute à array_zero une valeur de 0 à 15 :
                            //          col0    col1    col2    col3    
                            // Ligne 0    0       1       2       3
                            // Ligne 1    4       5       6       7
                            // Ligne 2    8       9      10      11
                            // Ligne 3   12      13      14      15
                            array_zero.Append(i + j%4);
                        }
                    }
                }
            }
            else
            {
                return null;
            }

            return array_zero;
            */
        }

        // Crée une tuile de valeur 2 ou 4 si un changement a eu lieu suite à une tentative de tassement
        // "Yeux fermés" : bof
        // "Fonction qui retourne les cases vides" puis tirage parmi la liste : mieux. Retourne [1, 3, 7, ...] ou [[0,1], [0,3], [1,7], ...]
        private void fnCreateTile(List<int> liste)
        {
            
            int index_random = random.Next(liste.Count);

            // Maintenant on veut mettre un 2 dans la tuile de memory_array qui correspond à la valeur stockée à l'index index_random de array
            int position = liste[index_random];
            int ligne = position / 4;
            int col = position % 4;

            // Déterminer (au hasard, 1 chance sur 8) si on créer un 2 ou un 4

            int resultRandom = random.Next(9);
            if(resultRandom == 0)
            {
                array_memory[ligne, col] = 4;
            }else
            {
                array_memory[ligne, col] = 2;
            }
            
            /*
            do
            {
                line = random.Next(4);
                row = random.Next(4);
            } while (array_memory[line,row] != 0);

            array_memory[line,row] = 2;
            */
        }

        // Regarde s'il y a un 2048 dans la grille. Si oui, message de fin de partie avec choix entre continuer et commencer une nouvelle partie
        private void fnCheckVictory()
        {
            // Vérifier s'il y a un 2048 dans la grille
            for (int i = 0; i < 4; i++)              // Lignes
            {
                for (int j = 0; j < 4; j++)          // Colonnes
                {
                    if(array_memory[i, j] == 2048)
                    {
                        hasWon = true;
                    }
                }
            }

            // Faire un choix: continuer ou commencer une nouvelle partie ?
            if (hasWon == true)
            {
                // Mettra la valeur DialogResult.Yes ou DialogResult.No dans la variable answer
                // (Pourquoi pas à remplacer par un nouveau form pour plus de liberté...)
                DialogResult answer = MessageBox.Show("Vous avez gagné !\nVoulez-vous continuer la partie ou en commencer une nouvelle ?\n\n" +
                                                      "Oui : Continuer la partie\nNon : Commencer une nouvelle partie",
                                                      "Félicitations !",
                                                      MessageBoxButtons.YesNo);

                if(answer == DialogResult.No)
                {
                    fnStartNewGame();
                }
            }
        }

        // Regarde s'il y a des tuiles adjacentes de même valeur dans la grille. Si oui, retourne false. Si non, retourne true.
        private void fnCheckFail()
        {
            hasLost = true;
            // Recherche de paires avec lecture horizontale de la grille, ligne par ligne
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (array_memory[i, j] == array_memory[i, j + 1])
                    {
                        hasLost = false;
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
                        hasLost = false;
                    }
                }
            }

            // Si on a trouvé aucune paire, alors on a perdu (car la fonction n'est appelée que lorsqu'il n'y a déjà plus de cases vides)
            if (hasLost == true)
            {
                // Mettra la valeur DialogResult.Yes ou DialogResult.No dans la variable answer
                // (Pourquoi pas à remplacer par un nouveau form pour plus de liberté...)
                DialogResult answer = MessageBox.Show("Vous avez perdu...\nVoulez-vous voir la grille ou commencer une nouvelle partie ?\n\n" +
                                                      "Oui : Voir la grille\nNon : Commencer une nouvelle partie",
                                                      "Dommage !",
                                                      MessageBoxButtons.YesNo);

                if (answer == DialogResult.No)
                {
                    fnStartNewGame();
                }
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

            // On crée un label pour le score
            label_score.Bounds = new Rectangle(490, 150, 90, 15);
            label_score.BackColor = Color.Gray;
            label_score.Text = "Score : ";
            label_score.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(label_score);

            // On crée un label pour le record
            label_record.Bounds = new Rectangle(490, 130, 90, 15);
            label_record.BackColor = Color.Gray;
            label_record.Text = "Record : ";
            label_record.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(label_record);
        }

        // Affiche le tableau mémoire de début de partie
        private void displayBeginningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hasWon = false;
            hasLost = false;
            fnCopy2DArray(array_example_beginning, array_memory);
            fnDisplay(array_memory);
        }

        // Affiche le tableau mémoire de milieu de partie avec tous les nombres possibles
        private void displayExampleInGameGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hasWon = false;
            hasLost = false;
            fnCopy2DArray(array_example_ingame, array_memory);
            fnDisplay(array_memory);

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

        // Affiche le tableau de mémoire de test
        private void displayExampleTestGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hasWon = false;
            hasLost = false;
            fnCopy2DArray(array_example_test, array_memory);
            fnDisplay(array_memory);
        }

        // e contient des infos sur l'évènement, dont le KeyCode (la touche appuyée)
        // Si on créer un bouton, écrire Form1_KeyDown(sender, e) dans Bouton1_Keydown pour transmettre l'info des touches appuyées à Form1
        // Ca marchera pour ASDW mais pas pour les flèches
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            int changes = 0;
            int changes_temp;
            int[] array_line;
            bool ligne;

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) // Tasser à gauche
            {
                ligne = true;
                for (int i = 0; i < 4; i++) // i = numéro de ligne
                {
                    array_line = fnMerge(array_memory[i, 0], array_memory[i, 1], array_memory[i, 2], array_memory[i, 3], out changes_temp);
                    changes += changes_temp;

                    // Affectation des résultats du tassement (array_line) au tableau mémoire du jeu (array_memory)
                    fnAffectationApresTassement(ligne, i, array_line[0], array_line[1], array_line[2], array_line[3]);
                }
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) // Tasser à droite
            {
                ligne = true;
                for (int i = 0; i < 4; i++) // i = numéro de ligne
                {
                    array_line = fnMerge(array_memory[i, 3], array_memory[i, 2], array_memory[i, 1], array_memory[i, 0], out changes_temp);
                    changes += changes_temp;

                    fnAffectationApresTassement(ligne, i, array_line[3], array_line[2], array_line[1], array_line[0]);
                }
            }
            else if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up) // Tasser en haut
            {
                ligne = false;
                for (int i = 0; i < 4; i++) // i = numéro de colonne
                {
                    array_line = fnMerge(array_memory[0, i], array_memory[1, i], array_memory[2, i], array_memory[3, i], out changes_temp);
                    changes += changes_temp;

                    fnAffectationApresTassement(ligne, i, array_line[0], array_line[1], array_line[2], array_line[3]);
                }
            }
            else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down) // Tasser en bas
            {
                ligne = false;
                for (int i = 0; i < 4; i++) // i = numéro de colonne
                {
                    array_line = fnMerge(array_memory[3, i], array_memory[2, i], array_memory[1, i], array_memory[0, i], out changes_temp);
                    changes += changes_temp;

                    fnAffectationApresTassement(ligne, i, array_line[3], array_line[2], array_line[1], array_line[0]);
                }
            }

            List<int> list_zero = fnListCoordZero();

            // Si changement
            if (changes > 0)
            {
                lblChangements.Text = "Changements : oui";
                fnCreateTile(list_zero);
                fnDisplay(array_memory);
                list_zero = fnListCoordZero(); // MAJ de la liste pour la fonction fnCheckFail un peu plus bas

                if (hasWon == false)
                {
                    fnCheckVictory();
                }
            }
            else
            {
                lblChangements.Text = "Changements : non";
            }

            // Si changement ET aucun 0 ET partie pas encore perdue
            if (changes > 0 && list_zero.Count == 0 && hasLost == false)
            {
                fnCheckFail();
            }
        }

        // Activer ou désactiver le test du tassement
        private void enableTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtTuile1.Enabled)
            {
                fnActivationTest(false);
            }
            else
            {
                fnActivationTest(true);
            }
        }

        // Tassement pour le test
        private void btTasser_Click(object sender, EventArgs e)
        {
            int changes_temp;
            int[] array_test_after = fnMerge(Int32.Parse(txtTuile1.Text), Int32.Parse(txtTuile2.Text), Int32.Parse(txtTuile3.Text), Int32.Parse(txtTuile4.Text), out changes_temp);
            txtResult.Text = array_test_after[0].ToString() + " " + array_test_after[1].ToString() + " " + array_test_after[2].ToString() + " " + array_test_after[3].ToString();

            // Si la ligne pré-tassement est différente de la ligne post-tassement, incrémenter la variable changes
            if (changes_temp > 0)
            {
                lblChangements.Text = "Changements : oui";
            }
            else
            {
                lblChangements.Text = "Changements : non";
            }
        }

        // Créer une grille aléatoire de début de jeu
        private void startNewGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fnStartNewGame();
        }
    }
}