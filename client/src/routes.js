import {
    BOOKING_ROUTE,
    HISTORY_ROUTE, HISTORY_USER_ROUTE,
    LOGIN_ROUTE,
    MYBOOKING_ROUTE,
    REGISTRATION_ROUTE,
    TABLES_ROUTE
} from "./utils/urls";
import Registration from "./pages/Registration";
import Login from "./pages/Login";
import Booking from "./pages/Booking";
import Tables from "./pages/Tables";
import MyBooking from "./pages/MyBooking";
import HistoryBooking from "./pages/HistoryBooking";
import {Navigate} from "react-router-dom";
import HistoryUserBooking from "./pages/HistoryUserBooking";


export const publicRoutes = [
    {
        path: REGISTRATION_ROUTE,
        element: <Registration/>
    },

    {
        path: LOGIN_ROUTE,
        element: <Login/>
    },


    {
        path: "*",
        element: <Navigate to={LOGIN_ROUTE} replace />,
    }
]

export const userRoutes = [
    {
        path: BOOKING_ROUTE,
        element: <Booking/>
    },

    {
        path: MYBOOKING_ROUTE,
        element: <MyBooking/>
    },

    {
        path: HISTORY_USER_ROUTE,
        element: <HistoryUserBooking/>
    },

    {
        path: "*",
        element: <Navigate to={BOOKING_ROUTE} replace />,
    },

];

export const adminRoutes = [
    {
        path: TABLES_ROUTE,
        element: <Tables/>
    },

    {
        path: HISTORY_ROUTE,
        element: <HistoryBooking/>,
    },

    {
        path: "*",
        element: <Navigate to={TABLES_ROUTE} replace />,
    }
];