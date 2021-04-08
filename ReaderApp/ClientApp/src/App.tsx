import './App.scss';
import 'react-toastify/dist/ReactToastify.css';
import { Suspense, useEffect } from 'react';
import { useDispatch, useSelector } from 'store';
import { RouteInfo, routes } from 'routes';
import { fetchAccountInfo } from 'store/auth/actions';
import { Route, Switch } from 'react-router-dom';
import Helmet from 'react-helmet';
import LinearProgress from '@material-ui/core/LinearProgress';
import { ToastContainer } from 'react-toastify';
import { Header } from 'components/Header';

const RouteWithTitle = (props: RouteInfo) => (
    <>
        <Helmet>
            <title>Reader App &ndash; {props.title}</title>
        </Helmet>

        <Route {...props} />
    </>
);

export const App = () => {
    const dispatch = useDispatch();
    const isAuthenticated = useSelector((state) => state.auth.isAuthorized);
    const isGeneralLoading = useSelector((state) => state.app.isGeneralLoading);

    useEffect(() => {
        if (isAuthenticated) {
            dispatch(fetchAccountInfo());
        }
    }, [dispatch, isAuthenticated]);

    return (
        <div className="app">
            {isGeneralLoading && <LinearProgress className="linear-progress" />}

            <ToastContainer position="top-right" autoClose={3000} closeOnClick pauseOnFocusLoss />

            <Header />

            <Suspense fallback={null}>
                <Switch>
                    {routes.map((route, index) => (
                        <RouteWithTitle {...route} key={index} />
                    ))}
                </Switch>
            </Suspense>
        </div>
    );
};
