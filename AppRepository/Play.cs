using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System.Collections.Generic;
using TMC.DBConnections;
using System.Linq;
using System.Globalization;
using TMC.Models;

namespace TMC.AppRepository
{
    public static class Play
    {
        public static bool SaveUpcomingPlay(TBL_UPCOMINGPLAYS obj)
        {
            bool resp = false;

            if (obj != null)
                resp = new TMCDBContext().fn_SaveUpcomingPlay(obj);

            return resp;
        }
        public static bool DeleteUpcomingPlay(int objID)
        {
            bool resp = false;

            if (objID > 0)
                resp = new TMCDBContext().fn_DeleteUpcomingPlay(objID);

            return resp;
        }
        public static bool DeleteAllUpcomingPlay()
        {
            return new TMCDBContext().fn_DeleteAllPlays();
        }
        public static List<TBL_UPCOMINGPLAYS> fn_GetAllPlays()
        {
            return new TMCDBContext().fn_GetAllPlays();
        }
        public static List<TBL_GENREMASTER> fn_GetAllGenres()
        {
            return new TMCDBContext().fn_GetAllGenres();
        }
        public static List<TBL_LANGUAGEMASTER> fn_GetAllLanguages()
        {
            return new TMCDBContext().fn_GetAllLanguages();
        }
        public static List<TBL_CITYMASTER> fn_GetAllCities()
        {
            return new TMCDBContext().fn_GetAllCities();
        }
        public static TBL_PLAYSMASTER fn_SavePlay(TBL_PLAYSMASTER obj)
        {
            var resp = new TBL_PLAYSMASTER();

            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.Genre))
                {
                    string _strGenre = "";
                    foreach (var currGenre in obj.Genre.Split(new char[] { ',', ';', '/' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.IsNullOrEmpty(currGenre))
                        {
                            _strGenre += new TMCDBContext().fn_SaveGenre(currGenre.Trim()).ToString() + ",";
                        }
                    }
                    if (_strGenre.StartsWith(","))
                        _strGenre = _strGenre.Substring(1, _strGenre.Length);
                    while (_strGenre.EndsWith(","))
                        _strGenre = _strGenre.Substring(0, _strGenre.Length - 1);
                    if (!string.IsNullOrEmpty(_strGenre))
                        obj.Genre = _strGenre;
                }

                if (!string.IsNullOrEmpty(obj.LANGAUAGE))
                {
                    string _strLanguage = "";
                    foreach (var currLang in obj.LANGAUAGE.Split(new char[] { ',', ';', '/' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.IsNullOrEmpty(currLang))
                        {
                            _strLanguage += new TMCDBContext().fn_SaveLanguage(currLang.Trim()).ToString() + ",";
                        }
                    }
                    if (_strLanguage.StartsWith(","))
                        _strLanguage = _strLanguage.Substring(1, _strLanguage.Length);
                    while (_strLanguage.EndsWith(","))
                        _strLanguage = _strLanguage.Substring(0, _strLanguage.Length - 1);
                    if (!string.IsNullOrEmpty(_strLanguage))
                        obj.LANGAUAGE = _strLanguage;
                }

                if (!string.IsNullOrEmpty(obj.CITY))
                {
                    obj.CITY = new TMCDBContext().fn_SaveCity(obj.CITY.Trim()).ToString();
                }

                if (obj.ID > 0)
                {
                    if (string.IsNullOrEmpty(obj.IMAGEURL))
                        obj.IMAGEURL = new TMCDBContext().fn_GetSinglePlayByID(obj.ID, obj.CREATEDBY).IMAGEURL;
                }

                resp = new TMCDBContext().fn_SavePlay(obj);
            }

            return resp;
        }
        public static ViewSinglePlayModel fn_GetSinglePlayByID(int ID, int UserID)
        {
            var respObj = new ViewSinglePlayModel();
            if (ID > 0)
            {
                var playObj = new TBL_PLAYSMASTER();
                playObj = new TMCDBContext().fn_GetSinglePlayByID(ID, UserID);
                if (playObj != null)
                    if (playObj.ID > 0)
                    {
                        var genreList = ""; var languageList = ""; var sliderList = "";

                        if (!string.IsNullOrEmpty(playObj.Genre))
                        {
                            foreach (var currGenre in playObj.Genre.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (!string.IsNullOrEmpty(currGenre))
                                {
                                    var objID = 0;
                                    int.TryParse(currGenre, out objID);
                                    if (objID > 0)
                                    {
                                        genreList += "," + new TMCDBContext().fn_GetSingleGenreByID(objID).Genre;
                                    }

                                }
                            }
                            if (genreList.StartsWith(","))
                                genreList = genreList.Substring(1, genreList.Length - 1);
                            while (genreList.EndsWith(","))
                                genreList = genreList.Substring(0, genreList.Length - 1);
                        }

                        if (!string.IsNullOrEmpty(playObj.LANGAUAGE))
                        {
                            foreach (var currLang in playObj.LANGAUAGE.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (!string.IsNullOrEmpty(currLang))
                                {
                                    var objID = 0;
                                    int.TryParse(currLang, out objID);
                                    if (objID > 0)
                                    {
                                        languageList += "," + new TMCDBContext().fn_GetSingleLanguageByID(objID).LANGUAGEVAL;
                                    }

                                }
                            }
                            if (languageList.StartsWith(","))
                                languageList = languageList.Substring(1, languageList.Length - 1);
                            while (languageList.EndsWith(","))
                                languageList = languageList.Substring(0, languageList.Length - 1);
                        }

                        sliderList = playObj.IMAGEURL + "," + Slider.GetCommaSeparated(ID, SliderObjectType.Plays);
                        if (sliderList.StartsWith(","))
                            sliderList = sliderList.Substring(1, sliderList.Length);
                        while (sliderList.EndsWith(","))
                            sliderList = sliderList.Substring(0, sliderList.Length - 1);

                        int cityID = 0;
                        if (!string.IsNullOrEmpty(playObj.CITY))
                            int.TryParse(playObj.CITY, out cityID);

                        respObj = new ViewSinglePlayModel()
                        {
                            ABOUT_THEATRE_LINK = playObj.ABOUT_THEATRE_LINK,
                            ACTOR = playObj.ACTOR,
                            AGESUITABLEFOR = playObj.AGESUITABLEFOR,
                            DATECREATED = playObj.DATECREATED.ToString("dddd, dd MMMM yyyy"),
                            DIRECTOR = playObj.DIRECTOR,
                            DURATION = playObj.DURATION,
                            Genre = genreList,
                            ID = ID,
                            IMAGEURL = sliderList,
                            LANGAUAGE = languageList,
                            NUMBER_OF_SHOWS = playObj.NUMBER_OF_SHOWS,
                            PREMIERDATE = playObj.PREMIERDATE,
                            SYNOPSIS = playObj.SYNOPSIS,
                            TITLE = playObj.TITLE,
                            TRAILERLINK = playObj.TRAILERLINK,
                            WRITER = playObj.WRITER,
                            CITY = new TMCDBContext().fn_GetSingleCityByID(cityID).CITY,
                            CASTNCREDIT = playObj.CASTNCREDIT,
                            CENSORCERTIFICATE = playObj.CENSORCERTIFICATE,
                            GROUPFACEBOOK_HANDLEURL = playObj.GROUPFACEBOOK_HANDLEURL,
                            GROUPINFO = playObj.GROUPINFO,
                            GROUPINSTAGARAM_HANDLEURL = playObj.GROUPINSTAGARAM_HANDLEURL,
                            GROUPTITLE = playObj.GROUPTITLE,
                            GROUPTWITTER_HANDLEURL = playObj.GROUPTWITTER_HANDLEURL,
                            PLAYLINK = playObj.PLAYLINK,
                            SYNOPSIS_SOCIALHANDLES = playObj.SYNOPSIS_SOCIALHANDLES,
                            TECHRIDER = playObj.TECHRIDER
                        };
                    }
            }
            return respObj;
        }
        public static bool DeletePlay(int objID)
        {
            bool resp = false;

            if (objID > 0)
                resp = new TMCDBContext().fn_DeletePlay(objID);

            return resp;
        }
        public static bool fn_DeleteAllExistingPlays()
        {
            return new TMCDBContext().fn_DeleteAllExistingPlays();
        }
        public static List<TBL_PLAYSMASTER> fn_GetAllExistingPlays(int UserID, string _genres = "", string _langs = "", string _cities = "")
        {
            var obj = new List<TBL_PLAYSMASTER>();
            obj = new TMCDBContext().fn_GetAllExistingPlays(UserID);
            var outputObj = new List<TBL_PLAYSMASTER>();

            if (!string.IsNullOrEmpty(_genres))
            {
                foreach (var currPlay in obj)
                {
                    foreach (var currGenre in _genres.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (currPlay.Genre.Split(",").Contains(currGenre))
                            if ((outputObj.Where(x => x.ID == currPlay.ID).ToList().Count) == 0)
                                outputObj.Add(currPlay);
                    }
                }
            }

            if (!string.IsNullOrEmpty(_langs))
            {
                if (outputObj.Count > 0)
                    obj = obj.Where(x => outputObj.Select(y => y.ID).ToList().Contains(x.ID)).ToList();

                foreach (var currPlay in obj)
                {
                    var isTrue = false;
                    foreach (var currLang in _langs.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (currPlay.LANGAUAGE.Split(",").Contains(currLang))
                        {
                            isTrue = true;
                        }
                    }
                    if (!isTrue)
                        outputObj.Remove(currPlay);
                    else
                    {
                        if ((outputObj.Where(x => x.ID == currPlay.ID).ToList().Count) == 0)
                            outputObj.Add(currPlay);
                    }
                }
            }

            if (!string.IsNullOrEmpty(_cities))
            {
                if (outputObj.Count > 0)
                    obj = obj.Where(x => outputObj.Select(y => y.ID).ToList().Contains(x.ID)).ToList();

                foreach (var currPlay in obj)
                {
                    var isTrue = false;
                    foreach (var currCity in _cities.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (currPlay.CITY.Split(",").Contains(currCity))
                        {
                            isTrue = true;
                        }
                    }
                    if (!isTrue)
                        outputObj.Remove(currPlay);
                    else
                    {
                        if ((outputObj.Where(x => x.ID == currPlay.ID).ToList().Count) == 0)
                            outputObj.Add(currPlay);
                    }
                }
            }

            if (string.IsNullOrEmpty(_genres) && string.IsNullOrEmpty(_langs) && string.IsNullOrEmpty(_cities))
                outputObj = obj;

            return outputObj;
        }
    }
}
