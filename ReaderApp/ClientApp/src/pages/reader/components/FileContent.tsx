import { Button, Drawer } from '@material-ui/core';
import { useMemo, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'store';
import { addToLearned } from 'store/words';
import { fetchInfoAboutWord, fetchSaveLearnedWords } from 'store/words/actions';
import { useFile } from '../hooks/useFile';
import { useOnClickOutside } from '../hooks/useOnClickOutside';

type WordInfo = {
    word: string;
    definitions: string[];
    lemma: string;
    translations: string[];
};

export const FileContent = () => {
    const [content, setContent] = useState('');
    const [showHiglighted, setShowHighlighting] = useState(false);
    const unknownWords = useSelector((state) => state.words.unknownWords);
    const workingFile = useSelector((state) => state.words.workingFile);
    const dispatch = useDispatch();

    const ref = useRef(null);

    const [isOpeWordInfoBar, setIsWordInfoBar] = useState(false);
    const [isOpenContextMenu, setIsOpenContextMenu] = useState(false);

    const [contextMenuCoordinates, setContextMenuCoordinates] = useState({
        x: 0,
        y: 0,
    });

    const [wordInfo, setWordInfo] = useState<WordInfo | null>(null);

    useFile(setContent);

    useOnClickOutside(ref, () => {
        setIsOpenContextMenu(false);
    });

    const higlightedText = useMemo(() => {
        let resultContent = content;

        unknownWords.forEach((unknownWord) => {
            resultContent = resultContent.replaceAll(
                new RegExp(`(^|\\s)${unknownWord}(?=\\s|$)`, 'g'),
                `&nbsp;<span data-unknown-word>${unknownWord}</span>`
            );
        });

        return resultContent;
    }, [unknownWords, content]);

    const onSelection = (e: React.MouseEvent<HTMLParagraphElement, MouseEvent>) => {
        setIsOpenContextMenu(true);

        setContextMenuCoordinates({
            x: e.clientX,
            y: e.clientY,
        });

        // const selected = window.getSelection()?.toString().trim();

        // if (selected && unknownWords.includes(selected)) {
        //     dispatch(addToLearned(selected));
        // }
    };

    const toggleDrawer = (val: boolean) => (event: React.KeyboardEvent | React.MouseEvent) => {
        if (
            event.type === 'keydown' &&
            ((event as React.KeyboardEvent).key === 'Tab' ||
                (event as React.KeyboardEvent).key === 'Shift')
        ) {
            return;
        }

        if (val === false) setWordInfo(null);

        setIsWordInfoBar(val);
    };

    const showInfoAboutWord = async () => {
        const selected = window.getSelection()?.toString().trim();

        if (selected) {
            const { payload, type } = await dispatch(fetchInfoAboutWord(selected));

            if (type.includes('fulfilled')) {
                console.log(payload)
                    
                setWordInfo({
                    word: selected,
                    ...(payload as {
                        definitions: string[];
                        lemma: string;
                        translations: string[];
                    }),
                });

                setIsWordInfoBar(true);
                setIsOpenContextMenu(false);
            }
        }
    };

    return (
        <section className="file-content">
            {workingFile && (
                <header>
                    <Button
                        variant="contained"
                        onClick={() => setShowHighlighting((prev) => !prev)}
                    >
                        Toggle unknown words View
                    </Button>
                    <Button variant="contained" onClick={() => dispatch(fetchSaveLearnedWords())}>
                        Save learned words
                    </Button>

                    {/* <Button variant="contained" onClick={toggleDrawer(true)}>
                        Open learned words
                    </Button> */}
                </header>
            )}

            <Drawer open={isOpeWordInfoBar} onClose={toggleDrawer(false)}>
                <div className="word-info">
                    {wordInfo && (
                        <>
                            <h2>{wordInfo.word}</h2>
                            <hr />

                            <h3>Definitions:</h3>
                            <ul>
                                {wordInfo.definitions.map((definition) => (
                                    <li>{definition}</li>
                                ))}
                            </ul>
                            <hr />

                            <h3>Translations:</h3>
                            <ul>
                                {wordInfo.translations.map((translation) => (
                                    <li>{translation}</li>
                                ))}
                            </ul>
                            <hr />

                            <Button
                                variant="contained"
                                onClick={() => {
                                    if (wordInfo) {
                                        dispatch(addToLearned(wordInfo.word));
                                    }
                                }}
                            >
                                Save word
                            </Button>
                        </>
                    )}
                </div>
            </Drawer>

            {isOpenContextMenu && (
                <div
                    ref={ref}
                    className="context-menu"
                    style={{
                        top: `${contextMenuCoordinates.y}px`,
                        left: `${contextMenuCoordinates.x}px`,
                    }}
                >
                    <button onClick={showInfoAboutWord}>Show info about word</button>
                </div>
            )}

            <p
                className="file-content-text"
                onDoubleClick={onSelection}
                dangerouslySetInnerHTML={{ __html: showHiglighted ? higlightedText : content }}
            ></p>
        </section>
    );
};
