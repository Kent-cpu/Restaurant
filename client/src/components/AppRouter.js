import React, {useContext} from 'react';
import {adminRoutes, publicRoutes, userRoutes} from "../routes";
import {Route, Routes} from "react-router-dom";
import {ROLES} from "../utils/roles";
import {AuthContext} from "../contexts";

const AppRouter = () => {
    const {isAuth} = useContext(AuthContext);
    const {user} = useContext(AuthContext);

    if (!isAuth) {
        return <Routes>
            {
                publicRoutes.map(({path, element}) =>
                    <Route key={path} path={path} element={element}/>
                )
            }
        </Routes>
    }

    return (
        <Routes>
            {
                user.role === ROLES.USER ?
                    userRoutes.map(({path, element}) =>
                        <Route key={path} path={path} element={element}/>
                    )
                    :
                    adminRoutes.map(({path, element}) =>
                        <Route key={path} path={path} element={element}/>
                    )
            }
        </Routes>
    );
};

export default AppRouter;