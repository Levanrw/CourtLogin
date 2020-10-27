using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAB.LoginInCourt
{
    public partial class Settingcs : Form
    {
        public Settingcs()
        {
            InitializeComponent();
        }

       
        private void Settingcs_Load(object sender, EventArgs e)
        {
            try
            {
                using (var context = new Analytics_NewEntities())
                {

                    var courtsetting = context.CourtSettings.Select(c => c).ToList<CourtSetting>().FirstOrDefault();
                    if (courtsetting != null)
                    {
                        if (courtsetting.RandomStart.HasValue)
                        {
                            StartSecond.Text = courtsetting.RandomStart.ToString();
                        }
                        if (courtsetting.RandomEnd.HasValue)
                        {
                            EndSecond.Text = courtsetting.RandomEnd.ToString();
                        }
                        if (courtsetting.ProgramExitTime.HasValue)
                        {
                            Exitdate.Value = Convert.ToDateTime(courtsetting.ProgramExitTime.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"მონაცემთა ჩატვირთვისას მოხდა შეცდომა: {ex.Message}");
            }

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            TimeSpan timeSpan = Exitdate.Value.TimeOfDay;
            TimeSpan timeSpannow = DateTime.Now.TimeOfDay;
            if (timeSpan > timeSpannow)
            {
            }
            else
            {
            }

            try
            {
                using (var db = new Analytics_NewEntities())
                {
                    var courtsetting = db.CourtSettings.Select(c => c).ToList<CourtSetting>().FirstOrDefault();
                    if (courtsetting !=null)
                    {
                        var result = db.CourtSettings.SingleOrDefault(b => b.Id == courtsetting.Id);
                        if (result != null)
                        {
                            result.RandomStart = Convert.ToInt32(StartSecond.Text);
                            result.RandomEnd = Convert.ToInt32(EndSecond.Text);
                            result.ProgramExitTime = Exitdate.Value.TimeOfDay;
                            result.SysDate = DateTime.Now;
                            db.SaveChanges();
                        }
                        MessageBox.Show("მონაცემები წარმატებით განახლდა!");
                    }

                    else
                    {
                        var data = db.Set<CourtSetting>();
                        data.Add(new CourtSetting { RandomStart = Convert.ToInt32(StartSecond.Text), RandomEnd = Convert.ToInt32(EndSecond.Text), ProgramExitTime = Exitdate.Value.TimeOfDay,SysDate=DateTime.Now });
                        db.SaveChanges();
                        MessageBox.Show("მონაცემები წარმატებით შეინახა!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"დაფიქსირდა შეცდომა: {ex.Message}");
            }
            }

        private void StartSecond_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void EndSecond_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
