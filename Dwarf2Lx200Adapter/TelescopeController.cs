using Dwarf2Lx200Adapter;
using System.Globalization;

public class TelescopeController
{
    private static readonly Lazy<TelescopeController> _instance = new(() => new TelescopeController());

    public static TelescopeController Instance => _instance.Value;

    public event EventHandler<(double RA, double DEC)> CoordinatesReceived;

    private readonly TelescopeData _telescopeData;

    public double TargetDeclination { get; private set; }

    public double TargetRightAscension { get; private set; }

    private double Latitude { get; set; }

    private double Longitude { get; set; }

    private double _utcOffsetHours;

    private DateTime _localTime;

    private TelescopeController()
    {
        _telescopeData = new TelescopeData();
    }

    public void UpdateTelescopeData(double rightAscension, double declination)
    {
        _telescopeData.RightAscension = rightAscension;
        _telescopeData.Declination = declination;

        CoordinatesReceived?.Invoke(this, (rightAscension, declination));
    }

    public double GetCurrentRA()
    {
        return _telescopeData.RightAscension;
    }

    public double GetCurrentDEC()
    {
        return _telescopeData.Declination;
    }

    public async Task<string> HandleCommand(string command)
    {
        //Sets the latitude of the currently selected site.
        if (command.StartsWith(":St"))
        {
            if (double.TryParse(command.Substring(3, 2), out double degrees) &&
                double.TryParse(command.Substring(6, 2), out double minutes))
            {
                double sign = command[2] == '-' ? -1 : 1;
                Latitude = sign * (degrees + (minutes / 60));
                return "1";
            }
            else
            {
                return "0";
            }
        }
        //Sets the longitude of the currently selected site
        else if(command.StartsWith(":Sg"))
        {
            if (double.TryParse(command.Substring(3, 3), out double degrees) &&
                double.TryParse(command.Substring(7, 2), out double minutes))
            {
                Longitude = degrees + (minutes / 60);
                return "1";
            }
            else
            {
                return "0";
            }
        }
        else if (command.StartsWith(":SG"))
        {
            if (double.TryParse(command.Substring(3), out double utcOffsetHours))
            {
                _utcOffsetHours = utcOffsetHours;
                return "1";
            }
            else
            {
                return "0";
            }
        }

        // Set the local time
        else if (command.StartsWith(":SL"))
        {
            if (TimeSpan.TryParse(command.Substring(3), out TimeSpan time))
            {
                _localTime = _localTime.Date + time;
                return "1";
            }
            else
            {
                return "0";
            }
        }

        // Set the local date
        else if (command.StartsWith(":SC"))
        {
            if (DateTime.TryParseExact(command.Substring(3), "MM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                _localTime = date.Date + _localTime.TimeOfDay;

                return "1";
                return "1Updating Planetary Data#" + new string(' ', 30) + "#";
            }
            else
            {
                return "0";
            }
        }
        else if (command.StartsWith(":Sr"))
        {
            if (double.TryParse(command.Substring(3, 2), out double hours) &&
                double.TryParse(command.Substring(6, 2), out double minutes) &&
                double.TryParse(command.Substring(9, 2), out double seconds))
            {
                TargetRightAscension = hours + (minutes / 60) + (seconds / 3600);

                return "1";
            }
            else
            {
                return "0";
            }
        }
        else if (command.StartsWith(":Sd"))
        {
            if (double.TryParse(command.Substring(4, 2), out double degrees) &&
                double.TryParse(command.Substring(7, 2), out double minutes) &&
                double.TryParse(command.Substring(10, 2), out double seconds))
            {
                double sign = command[3] == '-' ? -1 : 1;
                TargetDeclination = sign * (degrees + (minutes / 60) + (seconds / 3600));
                
                //TODO 
                
                return "1";
            }
            else
            {
                return "0";
            }
        }

        else if (command == ":GR")
        {
            double ra = GetCurrentRA();
            TimeSpan raTimeSpan = TimeSpan.FromHours(ra);
            string res = $"+{raTimeSpan.Hours:D2}:{raTimeSpan.Minutes:D2}.{raTimeSpan.Seconds:D1}#";
            return res;
        }
        else if (command == ":MS")
        {
            int result = 0;
            // START MOVE
            result = await Dwarf2Client.Goto(Longitude, Latitude, TargetRightAscension, TargetDeclination, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "DWARF_GOTO_LX200" + DateTime.UtcNow.ToString("yyyyMMddHH:mm:ss"));
            if(result == 0)
            {
                _telescopeData.Declination = TargetDeclination;
                _telescopeData.RightAscension = TargetRightAscension;
            }

            //call rest
            return result.ToString();
        }
        else if (command == ":GD")
        {
            double dec = GetCurrentDEC();
            int sign = Math.Sign(dec);
            dec = Math.Abs(dec);
            int degrees = (int)dec;
            int minutes = (int)((dec - degrees) * 60);
            int seconds = (int)((((dec - degrees) * 60) - minutes) * 60);
            string res = $"{(sign >= 0 ? "+" : "-")}{degrees:D2}*{minutes:D2}#";
            return res;
        }
        else if (command == ":CM")
        {
            _telescopeData.Declination = TargetDeclination;
            _telescopeData.RightAscension = TargetRightAscension;
            return "Coordinates matched.#";
        }
        else if (command.StartsWith(":RM") || command.StartsWith(":RS") || command.StartsWith(":RC") || command.StartsWith(":RG"))
        {
            Dwarf2Client.init(Longitude, Latitude,  _localTime.AddHours(_utcOffsetHours).ToString("yyyy-MM-dd HH:mm:ss"), "DWARF_GOTO_LX200" + DateTime.UtcNow.ToString("yyyyMMddHH:mm:ss"));

            return "1";
            //return result==0?"1":"0";   
        }
        else if (command == ":GVP")
        {
            return "LX200 Custom#";
        }
        else if (command == ":GVN")
        {
            return "1.0.0#";
        }
        else if (command == ":GVD")
        {
            return "2023-05-05#";
        }
        else if (command == ":GVZ")
        {
            return "12:34:56#";
        }
        else
        {
            return "1";
        }
    }
}
