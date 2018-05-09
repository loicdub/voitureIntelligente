/*
 * Auteurs : Dubas, Pereira
 * Projet  : Voiture intelligente à capteurs de distance
 * Date    : 21.02.2018 
 * Version : 1.0
 */

using System;
using Microsoft.SPOT;
// A ajouter
using Microsoft.SPOT.Hardware;
using GHI.Pins;
using device = GHI.Pins.FEZSpider;

namespace teslaX
{
    public class Program
    {
        #region debug
        public static bool ifDebug;
        public static OutputPort debugLed;
        public static InputPort debugBtn;
        public static bool currentDebugBtnValue;
        public static bool oldDebugBtnValue;
        #endregion

        // Constantes
        const double VMAX = 200.0;
        const double VITESSE_MAX_G = 1535;
        const double VITESSE_MAX_D = 1520;

        // Entrées
        public static AnalogInput capteurFrontal;
        public static AnalogInput capteurGauche;
        public static AnalogInput capteurDroite;

        // Sorties
        public static PWM roueGauche;
        public static PWM roueDroite;

        public static void Main()
        {
            #region debug
            debugLed = new OutputPort(device.DebugLed, false);
            debugBtn = new InputPort(device.Socket6.Pin3, false, Port.ResistorMode.Disabled);
            ifDebug = false;
            currentDebugBtnValue = false;
            oldDebugBtnValue = false;
            #endregion

            // Initialisation des entrées
            capteurFrontal = new AnalogInput(device.Socket9.AnalogInput3);
            capteurGauche = new AnalogInput(device.Socket9.AnalogInput4);
            capteurDroite = new AnalogInput(device.Socket9.AnalogInput5);

            // Initialisation des sorties
            roueDroite = new PWM(device.Socket11.Pwm7, 2500, (int)VITESSE_MAX_D, PWM.ScaleFactor.Microseconds, false);
            roueGauche = new PWM(device.Socket11.Pwm8, 2500, (int)VITESSE_MAX_G, PWM.ScaleFactor.Microseconds, false);

            // Démarrage des roues
            roueGauche.Start();
            roueDroite.Start();

            while (true)
            {
                #region debug
                /*
                currentDebugBtnValue = debugBtn.Read();
                if (currentDebugBtnValue == false && oldDebugBtnValue == true)
                {
                    roueGauche.Duration++;
                }

                oldDebugBtnValue = currentDebugBtnValue;
                */
                #endregion

                // Calcul de la vitesse en fonction des données des capteurs 
                //double vitesseG = -((((capteurFrontal.Read() - 0.5) * 2.0) -0.5)- (capteurGauche.Read() - capteurDroite.Read())) / 2 * VMAX;
                //double vitesseD = ((((capteurFrontal.Read() - 0.5) * 2.0) -0.5)+ (capteurGauche.Read() - capteurDroite.Read())) / 2 * VMAX;


                double vitesseG = -(((capteurFrontal.Read() - 0.5) * 2.0) - 0.5) * VMAX;
                double vitesseD = (((capteurFrontal.Read() - 0.5) * 2.0) - 0.5) * VMAX;


                // Régulation des vitesses des roues
                roueGauche.Duration = (uint)(VITESSE_MAX_G + vitesseG);
                roueDroite.Duration = (uint)(VITESSE_MAX_D + vitesseD);

                #region debug (affichage des données)
                //Debug.Print("Capteurs : " + capteurDroite.Read() + " || " + capteurFrontal.Read() + " || " + capteurGauche.Read());
                Debug.Print("Moteurs : " + roueDroite.Duration.ToString() + " || " + roueGauche.Duration.ToString());
                #endregion
            }
        }
    }
}
