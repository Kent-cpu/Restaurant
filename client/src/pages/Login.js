import React, {useContext, useState} from 'react';
import {Button, Card, Container, Form} from "react-bootstrap";
import {Formik} from "formik";
import {ROLES} from "../utils/roles";
import {NavLink, useNavigate} from "react-router-dom";
import {AuthContext} from "../contexts";
import * as Yup from "yup";
import {login} from "../http/userApi";
import {BOOKING_ROUTE, REGISTRATION_ROUTE, TABLES_ROUTE} from "../utils/urls";

const Login = () => {
    const [error, setError] = useState("");
    const navigate = useNavigate();
    const {setUser, setIsAuth} = useContext(AuthContext);

    const validationSchema = Yup.object().shape({
        username: Yup.string().required('Обязательное поле'),
        password: Yup.string().min(6, 'Пароль должен содержать не менее 6 символов').required('Обязательное поле'),
    });


    const loginUser = async (values) => {
        try {
            const {username, password} = values;
            const {id, role} = await login(username, password);
            setUser({id, role});
            setIsAuth(true);
            if(role === ROLES.USER) {
                navigate(BOOKING_ROUTE);
            }else {
                navigate(TABLES_ROUTE);
            }
        } catch (e) {
            const errorMessage = e.response.data["emailOrPassword"];
            if(errorMessage) {
                setError(errorMessage);
            }
        }
    };

    return (
        <Container
            className="d-flex justify-content-center align-items-center"
            style={{height: "100vh"}}
        >
            <Card style={{width: 450}} className="p-4 border-1">
                <Formik
                    initialValues={{username: '', password: ''}}
                    validationSchema={validationSchema}
                    onSubmit={loginUser}>
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
                            <h2 className="m-auto mb-5">Вход</h2>

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

                            {error && <p className="text-danger text-center">{error}</p>}

                            <Button
                                type="submit"
                                disabled={isValidating}
                                style={{width: "70%"}}
                                className="m-auto"
                            >
                                Войти
                            </Button>

                            <p className="mt-3">Нужна учетная запись?
                                <NavLink to={REGISTRATION_ROUTE}>Зарегистрироваться!</NavLink>
                            </p>
                        </Form>
                    )}
                </Formik>
            </Card>
        </Container>
    );
};

export default Login;