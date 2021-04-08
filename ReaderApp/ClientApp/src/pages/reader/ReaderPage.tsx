import { Container } from '@material-ui/core';
import { PageType } from 'pages/types';
import { FileContent } from './components/FileContent';

const ReaderPage: PageType = () => {
    return (
        <Container className="reader-page" component="section" maxWidth="lg">
            <FileContent />
        </Container>
    );
};

export default ReaderPage;
