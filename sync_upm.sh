#!/bin/sh

# Constants
PKG_ROOT_DIR="Assets"
UPM_BRANCH="upm"
UPM_TEMP_BRANCH="upm_temp"
REMOTE_NAME="origin"
MASTER_BRANCH="master"
SAMPLES_DIR="Samples"
DOCUMENTATION_DIR="Documentation"
FILES_TO_MOVE=(
    "LICENSE"
    "README.md"
)
FILES_TO_RESTORE=(
    "LICENSE.meta"
    "README.md.meta"
)

# Define variables
tag_name="0.0.1"
create_upm_temp=0
will_create_tag=""

# Function to print formatted log message
print_log() {
    echo "======== $1 ========"
}

# Create branch function
create_branch() {
    print_log "Creating branch"

    # Check if the branch already exists
    upm_branch_exists=$(git rev-parse --verify "$UPM_BRANCH" 2>/dev/null)

    if [ -n "$upm_branch_exists" ]; then
        # If the upm branch already exists, create a temporary branch and merge it back to upm branch after making modifications on the temp branch.
        create_upm_temp=1

        echo "Branch $UPM_BRANCH already exists. Creating temporary branch $UPM_TEMP_BRANCH."

        # If the temp branch already exists, delete it first.
        upm_temp_branch_exists=$(git rev-parse --verify "$UPM_TEMP_BRANCH" 2>/dev/null)
        if [ -n "$upm_temp_branch_exists" ]; then
            echo "Branch $UPM_TEMP_BRANCH already exists, deleting."
            git branch -D "$UPM_TEMP_BRANCH"
        fi

        # Create temporary branch with the same directory structure
        git subtree split --prefix="$PKG_ROOT_DIR" --branch "$UPM_TEMP_BRANCH"
        echo "Created temporary branch $UPM_TEMP_BRANCH"

    else

        # Initial creation of upm branch
        git subtree split --prefix="$PKG_ROOT_DIR" --branch "$UPM_BRANCH"
        echo "Branch $UPM_BRANCH created successfully!"
    fi
}

# Checkout branch function
checkout_branch() {
    print_log "Checking out branch"

    # Check if there are any uncommitted changes
    if ! git diff-index --quiet HEAD --; then
        echo "There are uncommitted changes in the current branch."
        echo "Please commit your changes or stash them before switching branches."
        exit 1
    fi

    # If a temp branch is created, perform subsequent operations on the temp branch. Otherwise, perform them on the upm branch.
    if [ "$create_upm_temp" -eq 1 ]; then
        git checkout "$UPM_TEMP_BRANCH"
        echo "Checked out branch $UPM_TEMP_BRANCH successfully!"
    else
        git checkout "$UPM_BRANCH"
        echo "Checked out branch $UPM_BRANCH successfully!"
    fi
}

# Move files to master branch function
move_files() {
    print_log "Moving files"

    # Files added in the upm branch
    for file in "${FILES_TO_MOVE[@]}"; do
        if git show "$MASTER_BRANCH":"$file" &> /dev/null; then
            git checkout "$MASTER_BRANCH" -- "$file"
            git add "$file"
        else
            echo "$file is not found in $MASTER_BRANCH"
        fi
    done

    # Samples directory
    git mv "$SAMPLES_DIR" Samples~ &> /dev/null && rm "$SAMPLES_DIR".meta || echo "$SAMPLES_DIR is not found"

    # Documentation directory
    git mv "$DOCUMENTATION_DIR" Documentation~ &> /dev/null || echo "$DOCUMENTATION_DIR is not found"

    # Newly added files in the upm branch need to generate meta files
    if [ "$create_upm_temp" -eq 1 ]; then
        # If there is an old upm branch, use the old meta files to avoid changing GUIDs.
        for file in "${FILES_TO_RESTORE[@]}"; do
            if git show "$UPM_BRANCH":"$file" &> /dev/null; then
                git checkout "$UPM_BRANCH" -- "$file"
                git add "$file"
            else
                if [[ "$file" == *.meta ]]; then
                    echo "$file is not found in $UPM_BRANCH. Will create a new file."
                    file_name="${file%.meta}"
                    generate_meta_file "$file_name" 0
                    git add "$file"
                else
                    echo "$file is not found in $UPM_BRANCH."
                fi
            fi
        done
    else
        for file in "${FILES_TO_RESTORE[@]}"; do
            if [[ "$file" == *.meta ]]; then
                file_name="${file%.meta}"
                generate_meta_file "$file_name" 0
                git add "$file"
            fi
        done
    fi
}

# Generate GUID
generate_guid() {
    local platform=$(uname)
    local guid=""

    if [[ "$platform" == CYGWIN* || "$platform" == MINGW* ]]; then
        guid=$(powershell -Command "[guid]::NewGuid().ToString()")
    else
        guid=$(uuidgen)
    fi

    guid=$(echo "$guid" | tr -d - | tr '[:upper:]' '[:lower:]')
    echo "$guid"
}

# Generate .meta file similar to Unity
generate_meta_file() {
    # Parameter 1: Original file path
    # Parameter 2: Whether it is a folder (1 for folder, 0 for non-folder)

    local file_path=$1
    local is_folder=$2

    # Path for the generated .meta file
    local meta_file_path="${file_path}.meta"

    # Create the .meta file
    echo "fileFormatVersion: 2" > "$meta_file_path"
    echo "guid: $(generate_guid)" >> "$meta_file_path"

    if [ "$is_folder" -eq 1 ]; then
        echo "folderAsset: yes" >> "$meta_file_path"
    fi

    echo "DefaultImporter:" >> "$meta_file_path"
    echo "  externalObjects: {}" >> "$meta_file_path"
    echo "  userData: " >> "$meta_file_path"

    echo "Created .meta file for $file_path"
}

# Merge branch function
merge_branch() {
    print_log "Merging branch"

    # This step is only necessary when currently on the temp branch.
    current_branch=$(git symbolic-ref --short HEAD)
    if [[ $current_branch == "$UPM_BRANCH" ]]; then
        echo "Skipping merge step. Currently on the $UPM_BRANCH branch."
        return
    fi

    # Switch to the target branch
    git checkout "$UPM_BRANCH"

    # Merge temporary branch into the target branch, resolving conflicts by selecting "theirs" (temporary branch) changes
    git merge "$UPM_TEMP_BRANCH" -X theirs --no-edit -m "[sync upm]"

    # Squash the changes
    git reset --soft HEAD~$(git rev-list --count "$UPM_BRANCH".."$UPM_TEMP_BRANCH")
    git commit -m "[sync upm]"
    echo "Merged $UPM_TEMP_BRANCH into $UPM_BRANCH."

    # Delete temporary branch
    git branch -D "$UPM_TEMP_BRANCH"
    echo "Deleted temporary branch $UPM_TEMP_BRANCH."
}

# Commit function
perform_commit() {
    print_log "Performing commit"

    git commit -m "[sync upm]"
    echo "Files committed successfully!"
}

# Read version from a package.json file
get_version_from_package_json() {
    # Parameter 1: Path of package.json
    local package_file="$1"

    # Check if package.json file exists
    if [ -f "$package_file" ]; then
        # Extract version number using grep and awk
        local version=$(grep -Eo '"version":.*?[^\\]",' "$package_file" | awk -F'"' '{print $4}')

        # Return the version number
        echo "$version"
    else
        # File does not exist, return empty string
        echo ""
    fi
}

# Create tag function
create_tag() {
    print_log "Creating tag"

    tags=$(git tag)

    if [ -n "$tags" ]; then
        echo "Local tag list:"
        echo "$tags"
    else
        echo "No local tags"
    fi

    read -p "Create new tag? (y/n): " will_create_tag
    if [ "$will_create_tag" != "y" ]; then
        echo "No tag will be created."
        return
    fi

    # Read the tag name from the package.json file as the default tag name
    package_file="./package.json"

    default_tag=$(get_version_from_package_json "$package_file")

    echo -e "Do you want to use the version number \033[1m\033[33m$default_tag\033[0;39m as the new tag name? (y/n): "
    read use_default_tag

    if [ "$use_default_tag" == "y" ]; then
        tag_name="$default_tag"
    else
        read -p "Please enter the new tag name: " tag_name
    fi

    # Check if the tag already exists
    tag_exists=$(git rev-parse --verify "$tag_name" 2>/dev/null)

    if [ -n "$tag_exists" ]; then
        # Tag already exists, ask if it should be deleted

        git tag -d "$tag_name"
        echo "Tag $tag_name deleted successfully!"
    fi

    # Create new tag
    git tag "$tag_name"
    echo "Tag $tag_name created successfully!"
}

# Delete remote branch function
delete_remote_branch() {
    print_log "Deleting remote branch"

    existing_branch=$(git ls-remote --exit-code --heads "$REMOTE_NAME" "$UPM_BRANCH" 2>/dev/null)

    if [ $? -eq 0 ]; then
        # Remote branch exists, ask if it should be deleted
        read -p "Remote branch $UPM_BRANCH exists. Do you want to delete it? (y/n): " delete_remote_branch

        if [ "$delete_remote_branch" == "y" ]; then
            git push "$REMOTE_NAME" --delete "$UPM_BRANCH"
            echo "Remote branch $UPM_BRANCH deleted successfully!"
        else
            echo "Cancelled deleting remote branch $UPM_BRANCH."
            exit 1
        fi
    else
        echo "Remote branch $UPM_BRANCH does not exist."
    fi
}

# Delete remote tag function
delete_remote_tag() {
    print_log "Deleting remote tag"

    if [ "$will_create_tag" != "y" ]; then
        echo "No tag will be deleted."
        return
    fi

    existing_tag=$(git ls-remote --exit-code --tags "$REMOTE_NAME" "$tag_name" 2>/dev/null)

    if [ $? -eq 0 ]; then
        # Remote tag exists, ask if it should be deleted
        read -p "Remote tag $tag_name exists. Do you want to delete it? (y/n): " delete_remote_tag

        if [ "$delete_remote_tag" == "y" ]; then
            git push "$REMOTE_NAME" --delete "refs/tags/$tag_name"
            echo "Remote tag $tag_name deleted successfully!"
        else
            echo "Cancelled deleting remote tag $tag_name."
            exit 1
        fi
    else
        echo "Remote tag $tag_name does not exist."
    fi
}

# Push to remote function
push_to_remote() {
    print_log "Pushing to remote"

    git push "$REMOTE_NAME" "$UPM_BRANCH" -f

    if [ "$will_create_tag" == "y" ]; then
        git push "$REMOTE_NAME" "$tag_name"
        echo "Pushed branch $UPM_BRANCH and tag $tag_name to remote successfully!"
    else
        echo "Pushed branch $UPM_BRANCH to remote successfully!"
    fi
}

# Reset file status function
reset_file_status() {
    print_log "Resetting file status"

    # Switch back to the previous branch
    git checkout "$MASTER_BRANCH"

    # Reset the files to the state of the previous branch
    git reset --hard

    echo "File status reset successfully!"
}

# Main flow
create_branch
checkout_branch
move_files
perform_commit
merge_branch
create_tag
delete_remote_tag
push_to_remote
reset_file_status
