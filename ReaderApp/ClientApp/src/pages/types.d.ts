import { CSSProperties } from 'react';
import { RouteComponentProps } from 'react-router-dom';

interface PageTypeProps<P> extends RouteComponentProps<P> {
    className?: string;
    style?: CSSProperties;
}

export type PageType<P extends {} = {}> = (props: PageTypeProps<P>) => JSX.Element;
