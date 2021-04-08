import { useDispatch } from 'store';
import { fetchUploadFile } from 'store/words/actions';
import { useDropzone } from 'react-dropzone';
import { useEffect } from 'react';

export const FileContainer = () => {
    const dispatch = useDispatch();

    const { acceptedFiles, getRootProps, getInputProps } = useDropzone();

    useEffect(() => {
        if (acceptedFiles.length) {
            dispatch(fetchUploadFile(acceptedFiles[0]));
        }
    }, [acceptedFiles, dispatch]);

    return (
        <div {...getRootProps({ className: 'dropzone' })}>
            <input {...getInputProps()} />
            <p>Drag 'n' drop some files here, or click to select files to read</p>
        </div>
    );
};
