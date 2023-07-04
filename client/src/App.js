import {BrowserRouter} from "react-router-dom";
import AppRouter from "./components/AppRouter";
import {AuthContext} from "./contexts";
import jwt_decode from "jwt-decode";
import {useEffect, useState} from "react";

function App() {
    const [user, setUser] = useState(null);
    const [isAuth, setIsAuth] = useState(false);

    useEffect(() => {
        try {
            const token = localStorage.getItem("token");
            if(!token) {
                return;
            }

            const {id, role} = jwt_decode(token);
            if(id && role) {
                setUser({id, role});
                setIsAuth(true);
            }
        }catch (e) {
            console.log(e);
        }

    }, [localStorage.getItem("token")]);


    return (
        <AuthContext.Provider value={{user, setUser, isAuth, setIsAuth}}>
          <BrowserRouter>
            <AppRouter />
          </BrowserRouter>
        </AuthContext.Provider>
    );
}

export default App;