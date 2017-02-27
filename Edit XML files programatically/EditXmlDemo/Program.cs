using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace EditXmlDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // DT_PARTIC. Cambiamos las fechas desde el formato "AAAAMMDD" a "AAAA-MM-DD", para el tag : "BirthDate".
            // string path = @"C:\Desarrollo\Definitions\INITS\PT Messages Generator GOLD COAST\java\messages\ORIGINAL\LBO_DT_PARTIC.xml";
            // changeBirthDatePartic(path);

            /* DT_SCHEDULE. Cambiamos :
               <Discipline Code="LB">
                  <Gender Code="M">
                    <Event Code="004">
                      <Phase Code="1" Type="3">
                        <Unit Code="02" PhaseType="0" .... />
             a
               <Discipline Code="LB">
                  <Gender Code="M">
                    <Event Code="004">
                      <Phase Code="1" Type="3">
                        <Unit Code="LBM004102" PhaseType="3" .... /> // PhaseType=Type
            */
            string path = @"C:\Desarrollo\Definitions\INITS\PT Messages Generator GOLD COAST\java\messages\ORIGINAL\LBO_DT_SCHEDULE.xml";
            changeRSCCodeStructureSchedule(path);
        }

        /////////////////////////////////////////////////////////
        // Change DT_SCHEDULE.
        static void changeRSCCodeStructureSchedule(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode root = doc.DocumentElement;
            XmlNode node = root.SelectSingleNode("/OdfBody/Competition");
            foreach (XmlNode nodeChild in node.ChildNodes)
            {
                if (nodeChild.Attributes == null)
                    continue;

                string text = nodeChild.Name; //or loop through its children as well
                var attCode = nodeChild.Attributes["Code"];
                if (attCode == null)
                    continue;

                if (text == "Discipline")
                {
                    string sDisciplina = attCode.Value;
                    addDisciplineSchedule(nodeChild, sDisciplina);
                }
            }
            doc.Save(path);
        }
        
        static void addDisciplineSchedule(XmlNode node, string discipline)
        {
            string RSCCode = discipline;
            foreach (XmlNode nodeChild in node.ChildNodes)
            {
                if (nodeChild.Attributes == null)
                    continue;

                string text = nodeChild.Name; //or loop through its children as well
                var attCode = nodeChild.Attributes["Code"];
                if (attCode == null)
                    continue;

                if (text == "Gender")
                {
                    string sGender = attCode.Value;
                    addGenderSchedule(nodeChild, RSCCode+sGender);
                }
            }
        }

        static void addGenderSchedule(XmlNode node, string discGender)
        {
            string RSCCode = discGender;
            foreach (XmlNode nodeChild in node.ChildNodes)
            {
                if (nodeChild.Attributes == null)
                    continue;

                string text = nodeChild.Name; //or loop through its children as well
                var attCode = nodeChild.Attributes["Code"];
                if (attCode == null)
                    continue;

                if (text == "Event")
                {
                    string sEvent = attCode.Value;
                    addEventSchedule(nodeChild, RSCCode + sEvent);
                }
            }
        }

        static void addEventSchedule(XmlNode node, string disGenEvent)
        {
            string RSCCode = disGenEvent;
            foreach (XmlNode nodeChild in node.ChildNodes)
            {
                if (nodeChild.Attributes == null)
                    continue;

                string text = nodeChild.Name; //or loop through its children as well
                var attCode = nodeChild.Attributes["Code"];
                var attType = nodeChild.Attributes["Type"];
                if (attCode == null)
                    continue;

                if (text == "Phase")
                {
                    string sPhase = attCode.Value;
                    string sType = attType.Value;
                    addPhaseSchedule(nodeChild, RSCCode+sPhase, sType);
                }
            }
        }

        static void addPhaseSchedule(XmlNode node, string disGenEvPhase, string sType)
        {
            string RSCCode = disGenEvPhase;
            foreach (XmlNode nodeChild in node.ChildNodes)
            {
                if (nodeChild.Attributes == null)
                    continue;

                string text = nodeChild.Name; //or loop through its children as well
                var attCode = nodeChild.Attributes["Code"];
                var attPhaseType = nodeChild.Attributes["PhaseType"];
                if (attCode == null)
                    continue;

                if (text == "Unit")
                {
                    string sUnit = attCode.Value;
                    attCode.Value = RSCCode + sUnit;
                    if (attPhaseType != null)
                        attPhaseType.Value = sType;
                }
            }
        }
        // Change DT_SCHEDULE.
        /////////////////////////////////////////////////////////



        
        /////////////////////////////////////////////////////////
        // Change DT_PARTIC
        static void changeBirthDatePartic(string path)
        {
            // DT_PARTIC. Cambiamos las fechas desde el formato "AAAAMMDD" a "AAAA-MM-DD", para el tag : "BirthDate".
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode root = doc.DocumentElement;
            XmlNode node = root.SelectSingleNode("/OdfBody/Competition");
            foreach (XmlNode nodeChild in node.ChildNodes)
            {
                string text = nodeChild.Name; //or loop through its children as well
                if (text=="Participant" && nodeChild.Attributes!=null)
                {
                    var birthDate = nodeChild.Attributes["BirthDate"];
                    if (birthDate != null)
                    {
                        // Cambiamos las fechas desde el formato "AAAAMMDD" a "AAAA-MM-DD".
                        // Para el tag : "BirthDate". 
                        string oldBirthDate = birthDate.Value;
                        if (oldBirthDate.Substring(4, 1) != "-")
                        {
                            string newDate = oldBirthDate.Substring(0, 4);
                            newDate += "-";
                            newDate += oldBirthDate.Substring(4, 2);
                            newDate += "-";
                            newDate += oldBirthDate.Substring(6, 2);
                            birthDate.Value = newDate;
                        }
                    }
                }
            }
            doc.Save(path);
        }
        // Change DT_PARTIC
        /////////////////////////////////////////////////////////
    }
}
