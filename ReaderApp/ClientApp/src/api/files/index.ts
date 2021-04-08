import { del, get, post } from "services/HttpService";
import { PostFileResponse, UserFileInfo } from "./types";

export const postTextFile = async (file: File) => {
    const formData = new FormData();
    formData.append(
        "file",
        file,
        file.name
    );
    const { data } = await post<PostFileResponse>("/api/Files", formData);

    return data;
}

export const getUserFiles = (userId: string) => get<UserFileInfo[]>("/api/Files/all", { params: { userId } });

export const deleteFile = (fileId: string) => del(`/api/Files/${fileId}`);

export const loadFile = (fileId: string) => get<Blob>(`api/Files/load/${fileId}`, { responseType: 'blob' });