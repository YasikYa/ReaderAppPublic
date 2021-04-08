import { useEffect } from 'react';
import { useDispatch, useSelector } from 'store';
import { fetchFiles, fetchRemoveFile } from 'store/files/actions';
import { setReadFile } from 'store/words';
import Button from '@material-ui/core/Button';
import { useHistory } from 'react-router';
import { paths } from 'routes/paths';
import { randomInteger } from 'lib';

export const FilesList = () => {
    const dispatch = useDispatch();
    const userId = useSelector((state) => state.auth.userInfo?.id);
    const userFiles = useSelector((state) => state.files.userFiles);

    const history = useHistory();

    useEffect(() => {
        if (userId) {
            dispatch(fetchFiles(userId));
        }
    }, [dispatch, userId]);

    return (
        <aside className="file-list">
            <h2>Files list</h2>

            {userFiles.length ? (
                <ul className="list">
                    {userFiles.map((file) => {
                        return (
                            <li key={file.id} className="list-item">
                                <div className="file">
                                    File name: <strong>{file.fileName}</strong>
                                    <br />
                                    {randomInteger(0, 1) ? (
                                        <strong style={{ color: '#e74c3c' }}>Complex</strong>
                                    ) : (
                                        <strong style={{ color: '#2ecc71' }}>Easy</strong>
                                    )}
                                </div>

                                <div className="actions">
                                    <Button
                                        color="primary"
                                        variant="contained"
                                        onClick={() => dispatch(fetchRemoveFile(file.id))}
                                    >
                                        Delete
                                    </Button>
                                    <Button
                                        color="primary"
                                        variant="contained"
                                        onClick={() => {
                                            dispatch(setReadFile(file));
                                            history.push(paths.READER);
                                        }}
                                    >
                                        Read now
                                    </Button>
                                </div>
                            </li>
                        );
                    })}
                </ul>
            ) : (
                <span>No files have been uploaded</span>
            )}
        </aside>
    );
};
