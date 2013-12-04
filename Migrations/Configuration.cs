namespace LeaveRequest.Migrations
{
    using LeaveRequest.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LeaveRequest.Models.RequestFormDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LeaveRequest.Models.RequestFormDB context)
        {
            //  This method will be called after migrating to the latest version.

            context.PayPeriods.AddOrUpdate(
                p => p.Period,
                new PayPeriod { Period = "12/23/12 to 01/05/13 - [13-01]" },
                new PayPeriod { Period = "01/06/13 to 01/19/13 - [13-02]" },
                new PayPeriod { Period = "01/20/13 to 02/02/13 - [13-03]" },
                new PayPeriod { Period = "02/03/13 to 02/16/13 - [13-04]" },
                new PayPeriod { Period = "02/17/13 to 02/02/13 - [13-05]" },
                new PayPeriod { Period = "03/03/13 to 03/16/13 - [13-06]" },
                new PayPeriod { Period = "03/17/13 to 03/30/13 - [13-07]" },
                new PayPeriod { Period = "03/31/13 to 04/13/13 - [13-08]" },
                new PayPeriod { Period = "04/14/13 to 04/27/13 - [13-09]" },
                new PayPeriod { Period = "04/28/13 to 05/11/13 - [13-10]" },
                new PayPeriod { Period = "05/12/13 to 05/25/13 - [13-11]" },
                new PayPeriod { Period = "05/26/13 to 06/08/13 - [13-12]" },
                new PayPeriod { Period = "06/09/13 to 06/22/13 - [13-13]" },
                new PayPeriod { Period = "06/23/13 to 07/06/13 - [13-14]" },
                new PayPeriod { Period = "07/07/13 to 07/20/13 - [13-15]" },
                new PayPeriod { Period = "07/21/13 to 08/03/13 - [13-16]" },
                new PayPeriod { Period = "08/04/13 to 08/17/13 - [13-17]" },
                new PayPeriod { Period = "08/18/13 to 08/31/13 - [13-18]" },
                new PayPeriod { Period = "09/01/13 to 09/14/13 - [13-19]" },
                new PayPeriod { Period = "09/15/13 to 09/28/13 - [13-20]" },
                new PayPeriod { Period = "09/29/13 to 10/12/13 - [13-21]" },
                new PayPeriod { Period = "10/13/13 to 10/26/13 - [13-22]" },
                new PayPeriod { Period = "10/27/13 to 11/09/13 - [13-23]" },
                new PayPeriod { Period = "11/10/13 to 11/23/13 - [13-24]" },
                new PayPeriod { Period = "11/24/13 to 12/07/13 - [13-25]" },
                new PayPeriod { Period = "12/08/13 to 12/21/13 - [13-26]" },
                new PayPeriod { Period = "12/22/13 to 01/04/14 - [14-01]" }
                //new PayPeriod { Period = "&nbsp;12/23/12 to 01/05/13&nbsp;&nbsp;&nbsp;&nbsp;[13-01]" },
                //new PayPeriod { Period = "&nbsp;01/06/13 to 01/19/13&nbsp;&nbsp;&nbsp;&nbsp;[13-02]" },
                //new PayPeriod { Period = "&nbsp;01/20/13 to 02/02/13&nbsp;&nbsp;&nbsp;&nbsp;[13-03]" },
                //new PayPeriod { Period = "&nbsp;02/03/13 to 02/16/13&nbsp;&nbsp;&nbsp;&nbsp;[13-04]" },
                //new PayPeriod { Period = "&nbsp;02/17/13 to 02/02/13&nbsp;&nbsp;&nbsp;&nbsp;[13-05]" },
                //new PayPeriod { Period = "&nbsp;03/03/13 to 03/16/13&nbsp;&nbsp;&nbsp;&nbsp;[13-06]" },
                //new PayPeriod { Period = "&nbsp;03/17/13 to 03/30/13&nbsp;&nbsp;&nbsp;&nbsp;[13-07]" },
                //new PayPeriod { Period = "&nbsp;03/31/13 to 04/13/13&nbsp;&nbsp;&nbsp;&nbsp;[13-08]" },
                //new PayPeriod { Period = "&nbsp;04/14/13 to 04/27/13&nbsp;&nbsp;&nbsp;&nbsp;[13-09]" },
                //new PayPeriod { Period = "&nbsp;04/28/13 to 05/11/13&nbsp;&nbsp;&nbsp;&nbsp;[13-10]" },
                //new PayPeriod { Period = "&nbsp;05/12/13 to 05/25/13&nbsp;&nbsp;&nbsp;&nbsp;[13-11]" },
                //new PayPeriod { Period = "&nbsp;05/26/13 to 06/08/13&nbsp;&nbsp;&nbsp;&nbsp;[13-12]" },
                //new PayPeriod { Period = "&nbsp;06/09/13 to 06/22/13&nbsp;&nbsp;&nbsp;&nbsp;[13-13]" },
                //new PayPeriod { Period = "&nbsp;06/23/13 to 07/06/13&nbsp;&nbsp;&nbsp;&nbsp;[13-14]" },
                //new PayPeriod { Period = "&nbsp;07/07/13 to 07/20/13&nbsp;&nbsp;&nbsp;&nbsp;[13-15]" },
                //new PayPeriod { Period = "&nbsp;07/21/13 to 08/03/13&nbsp;&nbsp;&nbsp;&nbsp;[13-16]" },
                //new PayPeriod { Period = "&nbsp;08/04/13 to 08/17/13&nbsp;&nbsp;&nbsp;&nbsp;[13-17]" },
                //new PayPeriod { Period = "&nbsp;08/18/13 to 08/31/13&nbsp;&nbsp;&nbsp;&nbsp;[13-18]" },
                //new PayPeriod { Period = "&nbsp;09/01/13 to 09/14/13&nbsp;&nbsp;&nbsp;&nbsp;[13-19]" },
                //new PayPeriod { Period = "&nbsp;09/15/13 to 09/28/13&nbsp;&nbsp;&nbsp;&nbsp;[13-20]" },
                //new PayPeriod { Period = "&nbsp;09/29/13 to 10/12/13&nbsp;&nbsp;&nbsp;&nbsp;[13-21]" },
                //new PayPeriod { Period = "&nbsp;10/13/13 to 10/26/13&nbsp;&nbsp;&nbsp;&nbsp;[13-22]" },
                //new PayPeriod { Period = "&nbsp;10/27/13 to 11/09/13&nbsp;&nbsp;&nbsp;&nbsp;[13-23]" },
                //new PayPeriod { Period = "&nbsp;11/10/13 to 11/23/13&nbsp;&nbsp;&nbsp;&nbsp;[13-24]" },
                //new PayPeriod { Period = "&nbsp;11/24/13 to 12/07/13&nbsp;&nbsp;&nbsp;&nbsp;[13-25]" },
                //new PayPeriod { Period = "&nbsp;12/08/13 to 12/21/13&nbsp;&nbsp;&nbsp;&nbsp;[13-26]" },
                //new PayPeriod { Period = "&nbsp;12/22/13 to 01/04/14&nbsp;&nbsp;&nbsp;&nbsp;[14-01]" }
            );

            context.LeaveTypes.AddOrUpdate(
                t => t.Type,
                new LeaveType { Type = "Sick leave for self" },
                new LeaveType { Type = "Sick leave for a dependent" },
                new LeaveType { Type = "Sick leave appointment/dependent" },
                new LeaveType { Type = "Vacation taken" },
                new LeaveType { Type = "Vacation buy taken" },
                new LeaveType { Type = "Floating holiday" },
                new LeaveType { Type = "In-Lieu time taken" },
                new LeaveType { Type = "Personal leave" },
                new LeaveType { Type = "Paid leave used" },
                new LeaveType { Type = "Leave without pay" },
                new LeaveType { Type = "Bereavement leave (5 days)" },
                new LeaveType { Type = "Other" },
                new LeaveType { Type = "Sick leave appontment/self" },
                new LeaveType { Type = "Jury duty" },
                new LeaveType { Type = "Sick leave combined" }
            );

            context.Managers.AddOrUpdate(
                m => m.LName,
                new Manager { LName = "Schmelzer", FName = "Barbara ", Email = "BSchmelzer@aclibrary.org", Title = "ADM Administrative Specialist II" },
                new Manager { LName = "Chadwick", FName = "Cindy", Email = "CChadwick@aclibrary.org", Title = "" },
                new Manager { LName = "Chattha", FName = "Darshan", Email = "DChattha@aclibrary.org", Title = "" },
                new Manager { LName = "Crosse", FName = "Ana", Email = "ACrosse@aclibrary.org", Title = "" },
                new Manager { LName = "Davis", FName = "Ronnie", Email = "RDavis@aclibrary.org", Title = "" },
                new Manager { LName = "DosSantos", FName = "Anthony", Email = "ADosSantos@aclibrary.org", Title = "" },
                new Manager { LName = "Edwards", FName = "Brian", Email = "BEdwards@aclibrary.org", Title = "" },
                new Manager { LName = "Fisher", FName = "Susan", Email = "SFisher@aclibrary.org", Title = "" },
                new Manager { LName = "Gomes", FName = "Rosemary", Email = "RGomes@aclibrary.org", Title = "" },
                new Manager { LName = "Greer", FName = "Paula", Email = "PGreer@aclibrary.org", Title = "" },
                new Manager { LName = "Hague", FName = "Rosa", Email = "RHague@aclibrary.org", Title = "" },
                new Manager { LName = "Harris", FName = "Linda", Email = "LHarris@aclibrary.org", Title = "" },
                new Manager { LName = "Harris2", FName = "Lisa", Email = "LHarris2@aclibrary.org", Title = "" },
                new Manager { LName = "Hofacket", FName = "Jean", Email = "JHofacket@aclibrary.org", Title = "" },
                new Manager { LName = "Iqbal", FName = "Asad", Email = "AIqbal@aclibrary.org", Title = "" },
                new Manager { LName = "Jouthas", FName = "Lee", Email = "LJouthas@aclibrary.org", Title = "" },
                new Manager { LName = "Kong", FName = "Luis", Email = "LKong@aclibrary.org", Title = "" },
                new Manager { LName = "Lancaster", FName = "Sally", Email = "SLancaster@aclibrary.org", Title = "" },
                new Manager { LName = "Marcille", FName = "Lorraine", Email = "LMarcille@aclibrary.org", Title = "" },
                new Manager { LName = "Martellacci", FName = "Laurie", Email = "LMartellacci@aclibrary.org", Title = "" },
                new Manager { LName = "Martirez", FName = "Louie", Email = "LMartirez@aclibrary.org", Title = "" },
                new Manager { LName = "McDevitt-Parks", FName = "Randy", Email = "RMcDevitt@aclibrary.org", Title = "" },
                new Manager { LName = "Mendez", FName = "Eileen", Email = "EMendez@aclibrary.org", Title = "" },
                new Manager { LName = "Moskovitz", FName = "Carolyn", Email = "CMoskovitz@aclibrary.org", Title = "" },
                new Manager { LName = "Navarrette", FName = "Marta", Email = "MNavarrette@aclibrary.org", Title = "" },
                new Manager { LName = "Nunes", FName = "Sandra", Email = "SNunes@aclibrary.org", Title = "" },
                new Manager { LName = "Oldenhage", FName = "Marion", Email = "MOldenhage@aclibrary.org", Title = "" },
                new Manager { LName = "Pacheco", FName = "Karen", Email = "KPacheco@aclibrary.org", Title = "" },
                new Manager { LName = "Pine", FName = "Sallie", Email = "SPine@aclibrary.org", Title = "" },
                new Manager { LName = "Ross", FName = "Sally", Email = "SRoss@aclibrary.org", Title = "" },
                new Manager { LName = "Smith", FName = "Tiona", Email = "TSmith@aclibrary.org", Title = "" },
                new Manager { LName = "Steel-Sabo", FName = "Kathy", Email = "KSteel@aclibrary.org", Title = "" },
                new Manager { LName = "Watson", FName = "Peggy", Email = "PWatson@aclibrary.org", Title = "" },
                new Manager { LName = "Wilson", FName = "Danielle", Email = "DWilson@aclibrary.org", Title = "" }
            );
        }
    }
}
