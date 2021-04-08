import { ComponentType, lazy } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { paths } from './paths';

// Lazy load all app pages
const HomePage = lazy(() => import('pages/home/HomePage'));
const LoginPage = lazy(() => import('pages/auth/LoginPage'));
const SignUpPage = lazy(() => import('pages/auth/SignUpPage'));
const ReaderPage = lazy(() => import('pages/reader/ReaderPage'));

export interface RouteInfo {
    path: string | string[];
    exact?: boolean;
    title: string;
    component: ComponentType<RouteComponentProps<any>> | ComponentType<any>;
}

export const routes: RouteInfo[] = [
    {
        path: paths.LOGIN,
        exact: true,
        component: LoginPage,
        title: 'Login',
    },
    {
        path: paths.HOME,
        exact: true,
        component: HomePage,
        title: 'ReaderApp',
    },
    {
        path: paths.SIGNUP,
        exact: true,
        component: SignUpPage,
        title: 'SignUp',
    },
    {
        path: paths.READER,
        exact: true,
        component: ReaderPage,
        title: 'Reader',
    },
];
