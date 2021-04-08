import { loadFile } from 'api/files';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'store';
import { fetchLoadFileUnknownWords } from 'store/words/actions';

export const useFile = (contentCallback: (readContent: string) => void) => {
    const dispatch = useDispatch();
    const workingFile = useSelector((state) => state.words.workingFile);

    useEffect(() => {
        if (workingFile?.fileInfo.id) {
            const { fileInfo, isUploaded } = workingFile;

            const fileReader = new FileReader();

            const callback = (e: ProgressEvent<FileReader>) => {
                const loadedText = e.target?.result as string;

                contentCallback(loadedText);
                
                isUploaded || dispatch(fetchLoadFileUnknownWords(fileInfo.id));
            };

            fileReader.addEventListener('load', callback);

            loadFile(fileInfo.id).then(({ data: fileData }) => {
                fileReader.readAsText(fileData);
            });

            return () => fileReader.removeEventListener('load', callback);
        } else {
            contentCallback('Select or upload file to read!');
        }
    }, [contentCallback, dispatch, workingFile]);
};
