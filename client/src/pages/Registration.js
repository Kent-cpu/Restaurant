import React, {useContext, useState} from 'react';
import {Button, Card, Container, Form} from "react-bootstrap";
import {ROLES} from "../utils/roles";
import {registration} from "../http/userApi";
import * as Yup from 'yup';
import {Formik} from "formik";
import {NavLink, useNavigate} from "react-router-dom";
import {AuthContext} from "../contexts";
import {BOOKING_ROUTE, LOGIN_ROUTE, TABLES_ROUTE} from "../utils/urls";

const Registration = () => {
    const [role, setRole] = useState(ROLES.USER);
    const [customError, setCustomError] = useState('');
    const navigate = useNavigate();
    const {setUser, setIsAuth} = useContext(AuthContext);

    const validationSchema = Yup.object().shape({
        username: Yup.string().required('Обязательное поле'),
        password: Yup.string().min(6, 'Пароль должен содержать не менее 6 символов').required('Обязательное поле'),
    });


    const registerUser = async (values) => {
        try {
            const {username, password} = values;
            const response = await registration(username, password, role);
            setUser({
                id: response.id,
                role: response.role,
            });
            setIsAuth(true);
            if(role === ROLES.USER) {
                navigate(BOOKING_ROUTE);
            }else {
                navigate(TABLES_ROUTE);
            }
        } catch (e) {
            if(e.response.data.status === 409) {
               setCustomError("Это имя пользователя зарегистрировано");
            }
        }
    };


    return (
        <Container
            className="d-flex justify-content-center align-items-center"
            style={{height: "100vh"}}
        >
            <Card style={{width: 450}} className="p-4">
                <Formik
                    initialValues={{username: '', password: ''}}
                    validationSchema={validationSchema}
                    onSubmit={registerUser}>
                    {({
                          values,
                          errors,
                          touched,
                          handleChange,
                          handleBlur,
                          handleSubmit,
                          isValidating,
                      }) => (
                        <Form onSubmit={handleSubmit} className="d-flex flex-column justify-content-center">
                            <h2 className="m-auto mb-4">Регистрация</h2>
                            <Form.Group className="d-flex justify-content-center mb-4">
                                <Form.Check
                                    type="radio"
                                    label="Пользователь"
                                    value={ROLES.USER}
                                    onChange={e => setRole(e.target.value)}
                                    className="pe-4"
                                    checked={role === ROLES.USER}
                                />

                                <Form.Check
                                    type="radio"
                                    label="Администратор"
                                    value={ROLES.ADMIN}
                                    onChange={e => setRole(e.target.value)}
                                    checked={role === ROLES.ADMIN}
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Control
                                    name="username"
                                    placeholder="Введите имя пользователя"
                                    value={values.username}
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    isInvalid={touched.username && errors.username}
                                />

                                <Form.Control.Feedback type="invalid">{errors.username}</Form.Control.Feedback>
                            </Form.Group>

                            <Form.Group className="mb-4">
                                <Form.Control
                                    type="password"
                                    placeholder="Введите пароль"
                                    name="password"
                                    value={values.password}
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    isInvalid={touched.password && errors.password}
                                />

                                <Form.Control.Feedback type="invalid">{errors.password}</Form.Control.Feedback>
                            </Form.Group>

                            {customError && <p className="text-danger text-center">{customError}</p>}

                            <Button
                                type="submit"
                                disabled={isValidating}
                                style={{width: "70%"}}
                                className="m-auto"
                            >
                                Зарегистрироваться
                            </Button>

                            <p className="mt-3">У вас уже есть учетная запись?
                                <NavLink to={LOGIN_ROUTE}> Войти!</NavLink>
                            </p>
                        </Form>
                    )}
                </Formik>
            </Card>
        </Container>
    );
};

export default Registration;