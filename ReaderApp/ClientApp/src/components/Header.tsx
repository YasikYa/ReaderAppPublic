import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Button from '@material-ui/core/Button';
import { Link } from 'react-router-dom';
import { paths } from 'routes/paths';
import { useSelector } from 'store';

export const Header = () => {
    const isAuthenticated = useSelector((state) => state.auth.isAuthorized);

    return (
        <AppBar position="static">
            <Toolbar>
                <nav className="header-nav">
                    <ul>
                        <li>
                            <Link to={paths.HOME}>Home</Link>
                        </li>
                        {!isAuthenticated && (
                            <li>
                                <Link to={paths.LOGIN}>Log in</Link>
                            </li>
                        )}
                        {!isAuthenticated && (
                            <li>
                                <Link to={paths.SIGNUP}>Sign up</Link>
                            </li>
                        )}
                    </ul>
                </nav>

                {isAuthenticated && <Button color="inherit">Log out</Button>}
            </Toolbar>
        </AppBar>
    );
};
