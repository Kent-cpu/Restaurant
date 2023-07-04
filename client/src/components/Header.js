import React, {useContext} from 'react';
import {Container, Nav, Navbar} from "react-bootstrap";
import {NavLink, useNavigate} from "react-router-dom";
import {
    BOOKING_ROUTE,
    HISTORY_ROUTE,
    HISTORY_USER_ROUTE,
    LOGIN_ROUTE,
    MYBOOKING_ROUTE,
    TABLES_ROUTE
} from "../utils/urls";
import {AuthContext} from "../contexts";
import {ROLES} from "../utils/roles";

const Header = () => {
    const {setUser, setIsAuth, user} = useContext(AuthContext);
    const navigate = useNavigate();

    const logout = () => {
        localStorage.removeItem('token');
        setUser(null);
        setIsAuth(false);
        navigate(LOGIN_ROUTE);
    }

    return (
        <Navbar className="mb-4" bg="primary" expand="lg">
            <Container>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav>
                        {
                            user.role === ROLES.USER ?
                                <>
                                    <NavLink
                                        style={{color: "white"}}
                                        className="nav-link fs-5 me-5"
                                        to={BOOKING_ROUTE}
                                    >Бронировать</NavLink>
                                    <NavLink
                                        style={{color: "white"}}
                                        className="nav-link fs-5 me-5"
                                        to={MYBOOKING_ROUTE}
                                    >Мои бронирования</NavLink>

                                    <NavLink
                                        style={{color: "white"}}
                                        className="nav-link fs-5 me-5"
                                        to={HISTORY_USER_ROUTE}
                                    >История бронирований</NavLink>
                                </>
                                :
                                <>
                                    <NavLink
                                        to={TABLES_ROUTE}
                                        style={{color: "white"}}
                                        className="nav-link fs-5 me-5">Столики</NavLink>
                                    <NavLink
                                        to={HISTORY_ROUTE}
                                        style={{color: "white"}}
                                        className="nav-link fs-5 me-5"
                                    >История бронирований</NavLink>
                                </>
                        }

                        <NavLink
                            to={LOGIN_ROUTE}
                            style={{color: "white"}}
                            className="nav-link fs-5 me-5"
                            onClick={logout}
                        >Выйти</NavLink>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default Header;