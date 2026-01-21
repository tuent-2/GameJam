using System;

public static class GameMsg
{
    public const string USERNAME_BLANK = "cmun";
    public const string PASSWORD_BLANK = "cmpc";
    public const string CONFIRM_PASSWORD_BLANK = "cmcp";
    public const string CONFIRM_NOT_EQUAL = "cmcp";
    public const string VERY_SIMPLE_PASSWORD = "cmvs";
    public const string USERNAME_OR_PASS_WRONG = "cmwu";
    public const string SERVER_LOGIN_ERROR = "cmsle";
    public const string REGISTER_LEAVE_BOARD = "cmrl";
    public const string UNREGISTER_LEAVE_BOARD = "cmurl";
    public const string NOT_ENOUGH_GOLD = "cmydg";
    public const string NOT_ENOUGH_DIAMOND = "cmydd";
    public const string NOT_VALID_CARD = "cmip";
    public const string NOT_SMALLEST_CARD = "cmns";
    public const string NOT_VALID_TURN = "cmwf";
    public const string DISCONNECT = "cmd";
    public const string ALREADY_PLAYING = "cmyaa";

    public const string LINKED_USER_IN_TABLE = "pulit";

    public const string INPUT_CAN_NOT_EMPTY = "cmicbl";
    public const string REPASSWORD_NOT_MATCH = "cmcpd";
    public const string AT_LEAST_6C = "cmnp6";
    public const string MOST_20C = "cmnp20";
    public const string CHANGE_PASSWORD_SUCCESS = "cmcps";

    public const string COPIED_USER_ID = "puic";
    public const string COPIED_BANK_NAME = "pbnc";
    public const string COPIED_BANK_ACCOUNT = "pbac";

    public const string REGISTER_INVALID_VERSION = "ev";
    public const string REGISTER_TOMANYACCOUNT = "ldv";
    public const string REGISTER_DAILYLIMIT = "ld";

    //Protect Account 
    public const string ACCOUNT_PROTECTED_SUCCESSFULLY = "paps";

    // HowHeyHow 
    public const string NOT_YET_TIME_TO_BET = "hhhcb";
    public const string PLAY_CAN_NOT_OUT = "hhhyc";

    // Game Slot
    public const string IS_AUTO_SPIN = "cmias";

    //Gift Code,   
    public const string GIFT_CODE_BLANK = "pgz";
    public const string GIFT_CODE_NOT_EXIST = "pgf";
    public const string GIFT_CODE_QUOTA_EXCEED = "pgb";
    public const string GIFT_CODE_EXPIRED = "pgc";
    public const string GIFT_CODE_NOT_VALID_CONDITION = "pgd";
    public const string GIFT_CODE_HAS_BEEN_CLAIMED = "pge";
    public const string GIFT_CODE_TYPE_WRONG = "pgf";
    public const string GIFT_CODE_WRONG_UID = "pgg";

    public const string EACH_DEVICE_ONE_GIFT = "pedc";

    //Mail
    public const string MAIL_ID_NOT_FOUND = "pmnf";
    public const string MAIL_REWARD_IS_RECEIVED = "pmrr";

    //Profile
    public const string PROFILE_INVALID = "ppi";

    // Payment

    public const string FILL_BANKNAME_BANKACCOUNT = "ppcyb";
    public const string TRANSACTION_PROCESSED = "ptibp";
    //public const string WITHDRAW_PROCESSED = "pywp";

    public const string PAYMENT_PACKAGE_ID_NOT_VALID = "ppnv";
    public const string PAYMENT_CREATE_ORDER_ERR = "ppty";

    public const string PAYMENT_ORDER_NOT_AVAILABLE_ERR =
        "ppty";

    public const string PAYMENT_BANK_PROVIDER_KEY_NOT_VALID = "pbkns";
    public const string PAYMENT_BANK_NUMBER_WITHDRAW_NOT_VALID = "pbnnv";
    public const string PAYMENT_AMOUNT_DIAMOND_NOT_VALID = "pdanv";
    public const string PAYMENT_WITHDRAW_PACKAGE_NOT_VALID = "pwpnv";
    public const string PAYMENT_UID_WITHDRAW_NOT_VALID = "punv";
    public const string PROCESS_WITHDRAW_ERROR = "pepwt";
    public const string VALIDATE_WITHDRAW_ERROR = "pevwi";
    public const string COMMIT_WITHDRAW_ERROR = "pewmis";

    public const string TRANSACTION_ID_NOT_VALID = "pitmr";

    public const string UID_MAPPING_SENDER_NOT_VALID = "pumnf";
    public const string BANK_NUMBER_SENDER_NOT_VALID = "piman";
    public const string QR_CODE_DOWNLOADED_SUCCESS = "pqrcds";

    public const string PAY_IN_TIME_OUT = "pcit";
    public const string PROCESS_PAY_IN_ERROR = "paeo";
    public const string NOT_ENOUGH_CONDITION = "pyane";
    public const string MISSION_NLD_NOT_COMPLETED = "pmwn";
    public const string DEPOSIT_UNLOCKED_BONNUS = "ppdtu";

    public const string HAVE_NOT_PLAY_ENOUGH_GAMES = "ppe19";
    public const string HAVE_REACHED_THE_WTHDRAW = "ppe20";
    public const string NOT_TIME_FOR_NEXT_DRAW = "ppe21";
    public const string REACHED_MONEY_LIMIT = "ppe22";
    public const string PLAY_MORE_GAME_TO_WITHDRAW = "pyntp";
    public const string WITH_DRAW_REQUEST_DAILY_LIMIT = "pwrf";
    public const string WITH_DRAW_CHECKING_ACCOUNT = "psew";


    public const string CHOOSE_WITHDRAW_PACKAGE = "ppcaw";
    public const string BANK_ACCOUNT_FIELD_EMPTY = "pbafc";

    //public const string DEPOSIT_SUCCESSFULL = "pds";

    // Create and Change board
    public const string CHANGE_BET_SUCCESSFUL = "cmcbs";

    public const string CAN_NOT_CHANGE_BET = "cmcnc";

    //Interact
    public const string SPECTATOR_CAN_NOT_INTERACT = "cmsc";

    //Pusoy
    public const string SINGLE = "pushc";
    public const string PAIR = "pusp";
    public const string TWO_PAIRS = "pustp";
    public const string THREE_OF_A_KIND = "pustk";
    public const string STRAIGHT = "puss";
    public const string FLUSH = "pusf";
    public const string FULL_HOUSE = "pusfh";
    public const string FOUR_OF_A_KIND = "pusfk";
    public const string STRAIGHT_FLUSH = "pustps";


    //ADs - Add Gold
    public const string ADS_NOT_READY = "cmai";
    public const string WATCHED_ALL_ADS = "cmyh";

    public const string GET_ALL_GOLD_BONUS = "pyhra";

    public const string GOLD_MUST_BELOW_300K = "pygmb";

    //Fishing Table
    public const string SKILL_IS_NOT_READY = "ftsk";

    public static string Join(string msg, params object[] list)
    {
        if (list == null) return msg;
        string[] parts = msg.Split('{');
        string result = parts[0];
        string part;
        string[] sub;
        int index;
        for (int i = 1; i < parts.Length; i++)
        {
            part = parts[i];
            sub = part.Split('}');
            index = Int32.Parse(sub[0]);
            if (list[index] == null) result += "{" + index + "?}";
            else result += list[index];
            result += sub[1];
        }

        return result;
    }
}