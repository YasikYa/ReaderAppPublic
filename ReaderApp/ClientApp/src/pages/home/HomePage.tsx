import { Container } from '@material-ui/core';
import { Redirect } from 'react-router-dom';
import { paths } from 'routes/paths';
import { useSelector } from 'store';
import { PageType } from '../types';
import { FilesList } from './components/FilesList';
import { FileContainer } from './components/FileContainer';
// import { FileContent } from './components/FileContent';

const HomePage: PageType = () => {
    const isAuthenticated = useSelector((state) => state.auth.isAuthorized);

    if (!isAuthenticated) {
        return <Redirect to={paths.LOGIN} />;
    }
    return (
        <Container className="home" component="section" maxWidth="lg">
            <FileContainer />
            
            <FilesList />
            {/* <FileContent /> */}
        </Container>
    );
};

export default HomePage;
