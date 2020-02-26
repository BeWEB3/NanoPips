﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public enum ErrorCode
    {
        INVALID_LOGIN=1001,
        EMAIL_ALREADY_EXIST = 1002,
        OTHER_ERROR = 1003,
        EMAIL_DOES_NOT_EXIST = 1004,
        INVALID_LINK = 1005,
        LINK_HAS_BEEN_EXPIRED = 1006,
        PLEASE_VERIFY_YOUR_EMAIL_ADDRESS = 1007,
        PLEASE_VERIFY_YOUR_EMAIL_ADDRESS_WE_HAVE_SEND_YOU_AN_EMAIL_AGAIN = 1008,
        INVALID_CODE = 1009,
        WALLET_ALREADY_CREATED = 1010,
        INSUFFICIANT_BALANCE = 1011,
        COINPAYMENT_EXCEPTION = 1012,
        WALLET_NOT_EXIST = 1013,
        NOT_FOUND = 1014,
        OLD_PASSWORD_IS_INVALID = 1015,
        ORDER_IS_NOT_PENDING = 1016,
        INSUFFICIANT_LIQUIDITY__PLEASE_CONTACT_SUPPORT = 1017,
        AMOUNT_IS_LESS_TO_TRADE = 1018,
        NODES_IS_UNDER_MAINTAINANCE_PLEASE_TRY_AGAIN_LATER = 1019,
        PLEASE_VERIFY_YOUR_EMAIL_ADDRESS_WE_HAVE_SENT_YOU_ANOTHER_EMAIL = 1020,
        CURRENCY_NOT_SUPPORTED = 1021,
        CURRENCY_NOT_FOUND = 1022,
    }
}
